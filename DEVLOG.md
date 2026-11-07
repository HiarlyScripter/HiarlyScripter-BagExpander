# DEVLOG — Bag Expander (HiarlyScripter)

## Contexto

Este mod foi desenvolvido como substituto 100% original ao `nickklmao-MoreInventorySlots` v1.0.3, que parou de funcionar com REPO v0.4.x. O código foi escrito do zero, sem reaproveitamento de código-fonte do mod original.

---

## O que estava quebrado no mod de referência

### Problema 1 — `KeyNotFoundException` em `Inventory_StartILHook`

**O que o mod original fazia:**
Usava um ILHook do MonoMod para modificar a instrução bytecode `ldc.i4 3` dentro do método `Inventory.Start`. Esse `3` representava o tamanho do loop que populava a lista de slots.

**Por que quebrou:**
Em REPO v0.4.x, o laço `for (int i = 0; i < 3; i++)` que inicializa os slots foi **movido de `Inventory.Start` para `Inventory.Awake`**. O ILHook apontava para `Start`, que não contém mais o `ldc.i4 3`. O cursor do ILHook não encontrava a instrução e lançava `KeyNotFoundException`.

**Como foi resolvido:**
Substituído por um Harmony **Postfix em `Inventory.Awake`** que, após o método original rodar, acessa o campo `inventorySpots` via reflexão (`AccessTools.Field`) e adiciona entradas `null` até atingir a quantidade configurada. Sem IL, sem fragilidade de cursor.

---

### Problema 2 — `allChildren` não encontrado em `InventoryUI`

**O que o mod original fazia:**
Buscava o campo `allChildren` diretamente em `InventoryUI` com reflexão estática inicializada no campo da classe.

**Por que quebrou:**
Em REPO v0.4.x, `InventoryUI` foi simplificado e o campo `allChildren` foi **movido para a classe base `SemiUI`**. O campo não existe mais diretamente em `InventoryUI`.

**Como foi resolvido:**
`AccessTools.Field(typeof(InventoryUI), "allChildren")` percorre a hierarquia de herança automaticamente, encontrando o campo em `SemiUI`. Nenhuma mudança de lógica — apenas a referência passou a funcionar corretamente.

---

### Problema 3 — Hook em `PunManager.SetItemNameLOGIC` procurava padrões IL inexistentes

**O que o mod original fazia:**
Usava um ILHook para interceptar padrões de variáveis locais (`ldloc 5`, `brfalse`) dentro de `SetItemNameLOGIC` para identificar quando um item era atribuído a um slot.

**Por que quebrou:**
O método `SetItemNameLOGIC` foi **completamente reescrito** em v0.4.x. Passou a usar dicionários explícitos (`playerInventorySpot1`, `playerInventorySpot2`, `playerInventorySpot3`) em vez de variáveis locais com padrões IL fixos. Nenhum dos padrões procurados existia mais.

**Como foi resolvido:**
Substituído por um Harmony **Postfix em `PunManager.SetItemNameLOGIC`** com lógica própria: rastreia os slots extras (índices ≥ 3) via `EquipTracker`, e ao detectar que um item foi nomeado, tenta reequipá-lo no slot correto. Sem dependência de estrutura interna do método.

---

### Problema 4 — Duplo disparo nos slots 1–3 no `InventorySpot.Update`

**O que o mod original fazia:**
Injetava código de leitura de teclas para todos os slots, incluindo 1, 2 e 3.

**Por que quebrou:**
Em v0.4.x, `InventorySpot.Update` passou a **tratar nativamente** as entradas `Inventory1`, `Inventory2`, `Inventory3`. O hook antigo causaria duplo disparo nesses slots.

**Como foi resolvido:**
O Postfix em `InventorySpot.Update` verifica `inventorySpotIndex >= 3` antes de qualquer ação, garantindo que só os slots novos (4–10) recebam o tratamento do mod.

---

## Resumo técnico das mudanças

| Componente | Antes (v0.2.x) | Depois (v0.4.x) — solução |
|---|---|---|
| Expansão de slots | ILHook em `Inventory.Start` | Harmony Postfix em `Inventory.Awake` |
| Campo `allChildren` | `InventoryUI` (direto) | `SemiUI` (base class) via AccessTools |
| Restaurar itens entre rodadas | ILHook com padrões IL frágeis | Harmony Postfix + `EquipTracker` próprio |
| Teclas de atalho | Todos os slots (1–10) | Apenas slots extras (4–10) |
| Framework de hook | MonoMod ILHooks + RuntimeDetour | Harmony 2.x Prefix/Postfix puro |

---

## Arquitetura do mod

- **`Core.cs`** — tudo em um arquivo: entrada BepInEx, configurações, `EquipTracker` (rastreio dos slots extras) e `SlotPatches` (7 patches Harmony). `SlotRegistry.cs` e `Patches.cs` nunca existiram na fonte atual.

---

## v1.0.6 — 2026-11-06 — build 26407089 (Unity 6000.3.17)

`InventoryUI` foi reescrita: `Awake` chama `LayoutSetup()` (guarda a lista de slots) e `LateUpdate` reposiciona só esses slots. O Postfix antigo em `Start` criava os clones tarde demais. Agora é um Prefix em `InventoryUI.Awake` que clona antes do layout, sem posição manual; rótulos via `InputGlyphText.SetTemplate`. Demais hooks verificados por decompilação (sem mudanças). Testado pelo autor: aprovado.
