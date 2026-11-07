# 🎒 Bag Expander

[![Versão](https://img.shields.io/badge/versão-1.0.6-brightgreen?style=flat-square)](https://thunderstore.io/c/repo/p/HiarlyScripter/BagExpander/)
[![R.E.P.O.](https://img.shields.io/badge/R.E.P.O.-Build%2026407089-blue?style=flat-square)](https://store.steampowered.com/app/3241660/REPO/)
[![BepInEx](https://img.shields.io/badge/BepInEx-5.4.23.5-yellow?style=flat-square)](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
[![Multiplayer](https://img.shields.io/badge/Multiplayer-Todos%20os%20jogadores-e74c3c?style=flat-square)]()
[![REPOConfig](https://img.shields.io/badge/REPOConfig-compatível-orange?style=flat-square)](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/)

> Cansado de só ter 3 slots? Com o **Bag Expander** você escolhe — de **0 a 10 slots** de inventário!

---

## ✨ O que faz

O **Bag Expander** expande (ou reduz) o número de slots do inventário do jogador. Configure livremente a quantidade ideal para o seu estilo de jogo e o da sua equipe.

---

## 🎒 Funcionalidades

- 📦 **Slots ajustáveis** — de 0 a 10 (padrão: 3); aplica ao reiniciar a rodada
- ⌨️ **Teclas automáticas** — slots 4 a 10 respondem às teclas `4` `5` `6` `7` `8` `9` `0`
- 👑 **Controle do host** — o host pode definir um limite máximo de slots para todos
- 🏠 **Itens na base** — itens nos slots extras podem permanecer na base ao trocar de rodada
- ⚙️ **REPOConfig** — todos os configs editáveis dentro do jogo *(opcional)*

---

## 👥 Quem precisa instalar?

> **Cada jogador precisa instalar o mod individualmente** para ter slots extras na sua máquina.

O host **não** cria slots extras para os clientes automaticamente — cada jogador controla seu próprio inventário.

| Cenário | Resultado |
|---|---|
| ✅ Só o host tem o mod | Só o host tem slots extras; clientes ficam com 3 |
| ✅ Todos têm o mod | Cada um tem seus próprios slots configurados |
| ✅ Todos têm + `HostControls = true` | Slots de todos limitados ao `SlotCount` do host |
| ❌ Ninguém tem o mod | Inventário padrão do jogo (3 slots) |

---

## ⚙️ Configurações

Edite em `BepInEx/config/com.hiarlyscripter.bagexpander.cfg` ou use o **REPOConfig** in-game.

| Seção | Chave | Padrão | Descrição |
|---|---|---|---|
| `Slots` | `SlotCount` | `3` | Número de slots de inventário (0–10). Reinicie a rodada para aplicar. |
| `Slots` | `LeaveInBase` | `false` | `true` = itens nos slots extras permanecem na base ao trocar de rodada |
| `Multiplayer` | `HostControls` | `true` | `true` = host define o número máximo de slots válido para todos na sala |

---

## 📦 Dependências

### Obrigatórias
| Mod | Versão testada | Link |
|---|---|---|
| BepInExPack | `5.4.23.5` | [Thunderstore](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/) |

### Opcionais
| Mod | Versão testada | Link | Para que serve |
|---|---|---|---|
| REPOConfig | `1.2.6` | [Thunderstore](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/) | Editar configs dentro do jogo sem abrir arquivos |

---

## 🖼️ Screenshots

*Screenshots em breve. Quer contribuir com prints do mod em ação? Entre em contato!*

<!-- SCREENSHOTS_PLACEHOLDER -->

---

## 🛠️ Instalação rápida

**Via r2modman (recomendado)**
1. Instale o **BepInExPack**
2. Procure e instale o **Bag Expander**
3. *(Opcional)* Instale o **REPOConfig**
4. Clique em **Start modded** e configure seus slots no `.cfg` ou in-game

**Via manual**
1. Instale o BepInExPack primeiro
2. Copie `plugins/HiarlyScripter-BagExpander/` para `BepInEx/plugins/`
3. Inicie o jogo — o arquivo de config é gerado automaticamente

---

## ❓ Problemas comuns

| Problema | Solução |
|---|---|
| Slots extras não aparecem na UI | Reinicie a rodada após alterar `SlotCount` |
| Clientes ficam com 3 slots mesmo com o mod | Cada jogador precisa instalar o mod individualmente |
| Host limitando meus slots | O host tem `HostControls = true` com um `SlotCount` menor que o seu |

---

*Mod criado por **[HiarlyScripter](https://discord.com/users/hiarly_ferreira)** — Testado com R.E.P.O. Build `26407089` · BepInEx `5.4.23.5`*
