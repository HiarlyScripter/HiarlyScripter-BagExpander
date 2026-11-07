# Guia de Instalação — Bag Expander

> ✅ Testado com **R.E.P.O. Build `26407089`** (2026-11-06) · **BepInExPack `5.4.23.5`** · **REPOConfig `1.2.6`** *(opcional)*
>
> ⚠️ Se o jogo atualizar e o mod parar de funcionar, verifique se há uma versão nova do Bag Expander compatível.

---

## Dependências

### Obrigatórias (instale antes do mod)

| Mod | Versão testada | Link | Para que serve |
|---|---|---|---|
| **BepInExPack** | `5.4.23.5` | [Thunderstore](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/) | Framework base — obrigatório para qualquer mod |

### Opcionais

| Mod | Versão testada | Link | Para que serve |
|---|---|---|---|
| **REPOConfig** | `1.2.6` | [Thunderstore](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/) | Editar as configurações do mod dentro do próprio jogo, sem precisar abrir arquivos |

---

## Como instalar

### Opção 1 — r2modman (recomendado)

1. Abra o **r2modman** e selecione o jogo **R.E.P.O.**
2. Clique em **Online** e procure por `BepInExPack` → instale
3. Procure por `Bag Expander` → instale
4. *(Opcional)* Procure por `REPOConfig` → instale
5. Clique em **Start modded** para jogar

### Opção 2 — Manual

1. Instale o **BepInExPack** primeiro
2. Copie a pasta `plugins/HiarlyScripter-BagExpander/` para dentro de `BepInEx/plugins/` no diretório do jogo
3. Inicie o jogo uma vez — o arquivo de config será gerado automaticamente em `BepInEx/config/com.hiarlyscripter.bagexpander.cfg`

---

## Quem precisa instalar?

> **Todos os jogadores** que quiserem ter slots extras precisam instalar o mod individualmente.

O host **não consegue** criar slots extras para jogadores que não têm o mod. Cada máquina renderiza sua própria interface de slots.

**Exceção:** se o host tiver `HostControls = true` (padrão), ele pode **limitar** quantos slots os clientes conseguem usar, mesmo que os clientes tenham configurado um número maior.

---

## Configurações disponíveis

Edite em `BepInEx/config/com.hiarlyscripter.bagexpander.cfg` ou use o **REPOConfig** dentro do jogo:

| Seção | Chave | Padrão | O que faz |
|---|---|---|---|
| `Slots` | `SlotCount` | `3` | Quantos slots você quer ter (0 a 10). Precisa reiniciar a rodada para aplicar. |
| `Slots` | `LeaveInBase` | `false` | Se `true`, itens nos slots extras ficam na base quando a rodada troca em vez de serem perdidos. |
| `Multiplayer` | `HostControls` | `true` | Se `true`, o host define o limite máximo de slots para todos na sala. |

---

## Problemas comuns

| Problema | Solução |
|---|---|
| Slots extras não aparecem | Certifique-se de que reiniciou a rodada após instalar/configurar |
| Outros jogadores não têm slots extras | Cada um precisa instalar o mod — slots não são compartilhados pela rede |
| Host bloqueou meus slots | O host tem `HostControls = true` e um `SlotCount` menor que o seu |

---

*Mod criado por **HiarlyScripter**.*
