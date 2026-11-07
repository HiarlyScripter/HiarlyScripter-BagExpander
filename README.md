# 🎒 Bag Expander

[![Thunderstore](https://img.shields.io/badge/Thunderstore-v1.0.6-brightgreen?style=flat-square&logo=thunderstore)](https://thunderstore.io/c/repo/p/HiarlyScripter/BagExpander/)
[![R.E.P.O.](https://img.shields.io/badge/R.E.P.O.-Build%2026407089-blue?style=flat-square)](https://store.steampowered.com/app/3241660/REPO/)
[![BepInEx](https://img.shields.io/badge/BepInEx-5.4.23.5-yellow?style=flat-square)](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
[![Licença](https://img.shields.io/badge/licença-crédito%20obrigatório-red?style=flat-square)](LICENSE)

> Cansado de só ter 3 slots? Com o **Bag Expander** você escolhe — de **0 a 10 slots** de inventário!

---

## ✨ O que faz

Expande (ou reduz) o número de slots do inventário do jogador. Cada jogador configura livremente a quantidade ideal para o seu estilo de jogo.

---

## 🎒 Funcionalidades

- 📦 **Slots ajustáveis** — de 0 a 10 (padrão: 3); aplica ao reiniciar a rodada
- ⌨️ **Teclas automáticas** — slots 4 a 10 respondem às teclas `4` `5` `6` `7` `8` `9` `0`
- 👑 **Controle do host** — o host pode definir um limite máximo de slots para todos
- 🏠 **Itens na base** — itens nos slots extras podem permanecer na base ao trocar de rodada
- ⚙️ **REPOConfig** — todos os configs editáveis dentro do jogo *(opcional)*

---

## 👥 Multiplayer

> **Cada jogador precisa instalar o mod individualmente** para ter slots extras.

| Cenário | Resultado |
|---|---|
| ✅ Só o host tem o mod | Só o host tem slots extras; clientes ficam com 3 |
| ✅ Todos têm o mod | Cada um tem seus próprios slots configurados |
| ✅ Todos têm + `HostControls = true` | Slots limitados ao `SlotCount` do host |
| ❌ Ninguém tem o mod | Inventário padrão do jogo (3 slots) |

---

## ⚙️ Configurações

| Seção | Chave | Padrão | Descrição |
|---|---|---|---|
| `Slots` | `SlotCount` | `3` | Número de slots (0–10). Reinicie a rodada para aplicar. |
| `Slots` | `LeaveInBase` | `false` | Itens nos slots extras permanecem na base ao trocar de rodada |
| `Multiplayer` | `HostControls` | `true` | Host define o número máximo de slots válido para todos |

---

## 📦 Instalação

**Via r2modman (recomendado):**
1. Instale o **BepInExPack**
2. Procure e instale o **Bag Expander** no Thunderstore
3. *(Opcional)* Instale o **REPOConfig**

**Via manual:**
1. Instale o BepInExPack
2. Copie `plugins/HiarlyScripter-BagExpander/` para `BepInEx/plugins/`

---

## 📄 Licença

[Licença customizada](LICENSE) — uso e estudo permitidos. **Crédito ao autor obrigatório** em qualquer redistribuição ou trabalho derivado.

---

*Mod criado por **[HiarlyScripter](https://discord.com/users/hiarly_ferreira)** · Testado com R.E.P.O. Build `26407089` · BepInEx `5.4.23.5`*
