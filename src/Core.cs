using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Photon.Pun;

namespace BagExpander
{
    [BepInPlugin("com.hiarlyscripter.bagexpander", "Bag Expander", "1.0.6")]
    public sealed class BagExpanderPlugin : BaseUnityPlugin
    {
        internal static BagExpanderPlugin Instance { get; private set; }
        internal static ManualLogSource Log        { get; private set; }

        // ── Seção: Slots ──────────────────────────────────────────────────────
        internal static ConfigEntry<int>  SlotCount;
        internal static ConfigEntry<bool> LeaveInBase;

        // ── Seção: Multiplayer ────────────────────────────────────────────────
        internal static ConfigEntry<bool> HostControls;

        private void Awake()
        {
            Instance = this;
            Log      = Logger;

            // ── Slots ──
            SlotCount = Config.Bind(
                "Slots", "SlotCount", 3,
                new ConfigDescription(
                    "Número de slots de inventário disponíveis. " +
                    "Mínimo: 0 (sem inventário), máximo: 10, padrão do jogo: 3. " +
                    "Requer reinício da rodada para aplicar. Editável pelo REPOConfig.",
                    new AcceptableValueRange<int>(0, 10)));

            LeaveInBase = Config.Bind(
                "Slots", "LeaveInBase", false,
                "true  = ao trocar de rodada, itens nos slots extras (acima de 3) ficam na base.\n" +
                "false = itens retornam equipados ao jogador na próxima rodada (padrão).");

            // ── Multiplayer ──
            HostControls = Config.Bind(
                "Multiplayer", "HostControls", true,
                "true  = o HOST define o número máximo de slots para todos os jogadores. " +
                "Clientes não conseguem equipar itens além do SlotCount do host, " +
                "mesmo que tenham o mod com um valor maior configurado.\n" +
                "false = cada jogador usa livremente seu próprio SlotCount. " +
                "Todos os jogadores precisam ter o mod instalado para se beneficiar.\n" +
                "IMPORTANTE: este mod deve ser instalado por cada jogador individualmente. " +
                "O host não consegue forçar slots extras em clientes que não têm o mod.");

            new Harmony("com.hiarlyscripter.bagexpander").PatchAll(typeof(SlotPatches));
            Log.LogInfo("Bag Expander v1.0.6 carregado.");
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  EquipTracker — rastreia itens nos slots extras entre rodadas
    // ──────────────────────────────────────────────────────────────────────────
    internal static class EquipTracker
    {
        private static readonly Dictionary<string, Dictionary<int, int>> _registry
            = new Dictionary<string, Dictionary<int, int>>();

        internal static void Record(string steamID, int slot, int nameHash)
        {
            if (!_registry.TryGetValue(steamID, out var slots))
            {
                slots = new Dictionary<int, int>();
                _registry[steamID] = slots;
            }
            slots[slot] = nameHash;
        }

        internal static void Erase(string steamID, int slot)
        {
            if (!_registry.TryGetValue(steamID, out var slots)) return;
            slots.Remove(slot);
            if (slots.Count == 0) _registry.Remove(steamID);
        }

        internal static void Reset() => _registry.Clear();

        internal static bool Lookup(int nameHash, out string steamID, out int slot, List<PlayerAvatar> players)
        {
            foreach (var player in players)
            {
                string sid = SemiFunc.PlayerGetSteamID(player);
                if (!_registry.TryGetValue(sid, out var slots)) continue;
                foreach (var kv in slots)
                {
                    if (kv.Value != nameHash) continue;
                    steamID = sid;
                    slot    = kv.Key;
                    return true;
                }
            }
            steamID = null;
            slot    = -1;
            return false;
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  SlotPatches — patches Harmony
    // ──────────────────────────────────────────────────────────────────────────
    [HarmonyPatch]
    internal static class SlotPatches
    {
        private static readonly FieldInfo _spotsListField =
            AccessTools.Field(typeof(Inventory), "inventorySpots");

        private static readonly MethodInfo _handleInputMethod =
            AccessTools.Method(typeof(InventorySpot), "HandleInput");

        // ── 1. Expande a lista interna de slots ───────────────────────────────
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Inventory), "Awake")]
        static void ExpandSlotList(Inventory __instance)
        {
            var list   = (List<InventorySpot>)_spotsListField.GetValue(__instance);
            int needed = Math.Max(BagExpanderPlugin.SlotCount.Value, 3);
            while (list.Count < needed) list.Add(null);
        }

        // ── 2. Ajusta a UI de slots ───────────────────────────────────────────
        // Prefixo em InventoryUI.Awake: os slots precisam existir ANTES do LayoutSetup()
        // do jogo, que guarda a lista de slots e reposiciona tudo no LateUpdate.
        // Assim o layout nativo posiciona os slots extras sozinho (sem posição manual).
        [HarmonyPrefix]
        [HarmonyPatch(typeof(InventoryUI), "Awake")]
        static void PrepareSlotsBeforeLayout(InventoryUI __instance)
        {
            int target = BagExpanderPlugin.SlotCount.Value;
            if (target == 3) return;

            var spots = ((Component)__instance).GetComponentsInChildren<InventorySpot>(true)
                .OrderBy(s => s.inventorySpotIndex).ToList();
            if (spots.Count == 0)
            {
                BagExpanderPlugin.Log.LogWarning("InventoryUI sem InventorySpot; slots não ajustados.");
                return;
            }

            if (target < 3)
            {
                for (int i = target; i < spots.Count; i++)
                    spots[i].gameObject.SetActive(false);
                return;
            }

            InventorySpot tpl = spots[0];
            for (int i = spots.Count; i < target; i++)
            {
                InventorySpot clone = UnityEngine.Object.Instantiate(tpl, tpl.transform.parent);
                clone.name = $"Inventory Spot {i + 1}";
                clone.inventorySpotIndex = i;
                clone.gameObject.SetActive(true);

                string label = (i == 9) ? "0" : (i + 1).ToString();
                foreach (var glyph in clone.GetComponentsInChildren<InputGlyphText>(true))
                    glyph.SetTemplate(label);
            }
            BagExpanderPlugin.Log.LogInfo($"Slots de inventário: {target} (UI criada antes do layout).");
        }

        // ── 3. Teclas de atalho para slots 4–10 ──────────────────────────────
        [HarmonyPostfix]
        [HarmonyPatch(typeof(InventorySpot), "Update")]
        static void ExtraSlotKeyInput(InventorySpot __instance)
        {
            int idx = __instance.inventorySpotIndex;
            if (idx < 3 || idx >= BagExpanderPlugin.SlotCount.Value) return;

            Key key = (idx == 9)
                ? Key.Digit0
                : (Key)Enum.Parse(typeof(Key), $"Digit{idx + 1}");

            if (((ButtonControl)Keyboard.current[key]).wasPressedThisFrame)
                _handleInputMethod.Invoke(__instance, null);
        }

        // ── 4. Registra itens nos slots extras (host/singleplayer) ────────────
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StatsManager), "PlayerInventoryUpdate")]
        static void TrackExtraEquip(string _steamID, string itemName, int spot)
        {
            if (!SemiFunc.IsMasterClientOrSingleplayer() || spot < 3) return;

            if (string.IsNullOrEmpty(itemName))
                EquipTracker.Erase(_steamID, spot);
            else
                EquipTracker.Record(_steamID, spot, itemName.GetHashCode());
        }

        // ── 5. Controla quem pode equipar além do limite ───────────────────────
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ItemEquippable), "RPC_RequestEquip")]
        static bool CheckSlotLimit(int spotIndex)
        {
            if (!SemiFunc.IsMultiplayer()) return true;
            if (!BagExpanderPlugin.HostControls.Value) return true;

            // HostControls=true: bloqueia qualquer equip além do SlotCount do host
            return spotIndex < BagExpanderPlugin.SlotCount.Value;
        }

        // ── 6. Limpa estado ao voltar ao menu ─────────────────────────────────
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuOpen), "Start")]
        static void ClearStateOnMenu() => EquipTracker.Reset();

        // ── 7. Restaura itens dos slots extras após troca de rodada ───────────
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PunManager), "SetItemNameLOGIC",
            new Type[] { typeof(string), typeof(int), typeof(ItemAttributes) })]
        static void RestoreExtraSlotItems(
            PunManager __instance, string _name, int photonViewID, ItemAttributes _itemAttributes)
        {
            if (!SemiFunc.IsMasterClientOrSingleplayer()) return;

            ItemAttributes attr = _itemAttributes;
            if (attr == null && SemiFunc.IsMultiplayer())
            {
                var pv = PhotonView.Find(photonViewID);
                if (pv == null) return;
                attr = pv.GetComponent<ItemAttributes>();
            }
            if (attr == null) return;

            var equippable = attr.GetComponent<ItemEquippable>();
            if (equippable == null) return;

            var players = SemiFunc.PlayerGetList();
            if (!EquipTracker.Lookup(_name.GetHashCode(), out string sid, out int slotIdx, players))
                return;

            if (!BagExpanderPlugin.LeaveInBase.Value)
            {
                int viewId = -1;
                if (SemiFunc.IsMultiplayer())
                {
                    var owner = players.FirstOrDefault(p => SemiFunc.PlayerGetSteamID(p) == sid);
                    if (owner != null) viewId = owner.photonView.ViewID;
                }
                equippable.RequestEquip(slotIdx, viewId);
            }

            EquipTracker.Erase(sid, slotIdx);
        }
    }
}
