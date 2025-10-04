# Estrutura de Pastas para Projetos Unity AAA

## Estrutura Recomendada para Visual Novel Engine (Padrão AAA)

```
Assets/
├── Scripts/
│   ├── Core/                           # Sistemas fundamentais
│   │   ├── Managers/                   # Singletons e sistemas principais
│   │   │   ├── GameManager.cs
│   │   │   ├── DialogueManager.cs
│   │   │   ├── CharacterManager.cs
│   │   │   ├── AudioManager.cs
│   │   │   ├── InputManager.cs
│   │   │   ├── SaveManager.cs
│   │   │   └── SceneManager.cs
│   │   ├── Systems/                    # Sistemas de gameplay
│   │   │   ├── Dialogue/
│   │   │   │   ├── DialogueSystem.cs
│   │   │   │   ├── DialogueParser.cs
│   │   │   │   ├── TextArchitect.cs
│   │   │   │   └── AutoReader.cs
│   │   │   ├── Characters/
│   │   │   │   ├── Character.cs
│   │   │   │   ├── CharacterFactory.cs
│   │   │   │   └── CharacterTypes/
│   │   │   │       ├── CharacterSprite.cs
│   │   │   │       ├── CharacterLive2D.cs
│   │   │   │       └── CharacterModel3D.cs
│   │   │   ├── Audio/
│   │   │   │   ├── AudioChannel.cs
│   │   │   │   ├── AudioTrack.cs
│   │   │   │   └── AudioMixerController.cs
│   │   │   └── Commands/
│   │   │       ├── CommandManager.cs
│   │   │       ├── CommandDatabase.cs
│   │   │       └── Commands/
│   │   │           ├── ShowCharacterCommand.cs
│   │   │           ├── PlayAudioCommand.cs
│   │   │           └── ChangeSceneCommand.cs
│   │   ├── Events/                     # Sistema de eventos
│   │   │   ├── GameEvents.cs
│   │   │   ├── DialogueEvents.cs
│   │   │   ├── AudioEvents.cs
│   │   │   └── EventBus.cs
│   │   └── Utils/                      # Utilitários core
│   │       ├── Extensions/
│   │       ├── Helpers/
│   │       └── Constants/
│   ├── Gameplay/                       # Lógica de gameplay
│   │   ├── Dialogue/
│   │   │   ├── UI/
│   │   │   │   ├── DialogueUI.cs
│   │   │   │   ├── DialogueBox.cs
│   │   │   │   ├── CharacterNameBox.cs
│   │   │   │   └── ChoicePanel.cs
│   │   │   ├── Data/
│   │   │   │   ├── DialogueData.cs
│   │   │   │   ├── DialogueLine.cs
│   │   │   │   └── DialogueContainer.cs
│   │   │   └── Controllers/
│   │   │       ├── DialogueController.cs
│   │   │       └── DialogueInputHandler.cs
│   │   ├── Characters/
│   │   │   ├── Controllers/
│   │   │   │   ├── CharacterController.cs
│   │   │   │   └── CharacterAnimationController.cs
│   │   │   ├── UI/
│   │   │   │   ├── CharacterUI.cs
│   │   │   │   └── CharacterPortrait.cs
│   │   │   └── Behaviors/
│   │   │       ├── CharacterMovement.cs
│   │   │       └── CharacterEffects.cs
│   │   └── Scenes/
│   │       ├── SceneController.cs
│   │       ├── SceneTransition.cs
│   │       └── BackgroundController.cs
│   ├── UI/                             # Interface do usuário
│   │   ├── Core/
│   │   │   ├── UIManager.cs
│   │   │   ├── UIPanel.cs
│   │   │   └── UIAnimation.cs
│   │   ├── Menus/
│   │   │   ├── MainMenu/
│   │   │   │   ├── MainMenuController.cs
│   │   │   │   └── MainMenuUI.cs
│   │   │   ├── Settings/
│   │   │   │   ├── SettingsController.cs
│   │   │   │   └── SettingsUI.cs
│   │   │   └── PauseMenu/
│   │   │       ├── PauseMenuController.cs
│   │   │       └── PauseMenuUI.cs
│   │   ├── HUD/
│   │   │   ├── DialogueHUD.cs
│   │   │   ├── CharacterHUD.cs
│   │   │   └── AudioHUD.cs
│   │   └── Components/
│   │       ├── ButtonExtensions.cs
│   │       ├── SliderExtensions.cs
│   │       └── TextExtensions.cs
│   ├── Audio/                          # Sistema de áudio
│   │   ├── Core/
│   │   │   ├── AudioManager.cs
│   │   │   ├── AudioSourcePool.cs
│   │   │   └── AudioSettings.cs
│   │   ├── Music/
│   │   │   ├── MusicController.cs
│   │   │   └── MusicTransition.cs
│   │   ├── SFX/
│   │   │   ├── SFXController.cs
│   │   │   └── SFXPool.cs
│   │   └── Voice/
│   │       ├── VoiceController.cs
│   │       └── VoiceSettings.cs
│   ├── Input/                          # Sistema de input
│   │   ├── InputManager.cs
│   │   ├── InputActions/
│   │   │   ├── DialogueInputActions.cs
│   │   │   ├── MenuInputActions.cs
│   │   │   └── GameplayInputActions.cs
│   │   └── Controllers/
│   │       ├── KeyboardInputController.cs
│   │       ├── GamepadInputController.cs
│   │       └── TouchInputController.cs
│   ├── Data/                           # Dados e configurações
│   │   ├── ScriptableObjects/
│   │   │   ├── GameSettings.cs
│   │   │   ├── DialogueSettings.cs
│   │   │   ├── AudioSettings.cs
│   │   │   └── CharacterSettings.cs
│   │   ├── SaveSystem/
│   │   │   ├── SaveData.cs
│   │   │   ├── SaveManager.cs
│   │   │   └── SaveSlots.cs
│   │   └── Localization/
│   │       ├── LocalizationManager.cs
│   │       ├── LanguageData.cs
│   │       └── TextLocalizer.cs
│   ├── Performance/                    # Otimizações e performance
│   │   ├── ObjectPooling/
│   │   │   ├── ObjectPool.cs
│   │   │   ├── PooledObject.cs
│   │   │   └── AudioSourcePool.cs
│   │   ├── Memory/
│   │   │   ├── MemoryManager.cs
│   │   │   └── ResourceManager.cs
│   │   └── Profiling/
│   │       ├── PerformanceProfiler.cs
│   │       └── MemoryProfiler.cs
│   ├── Tools/                          # Ferramentas de desenvolvimento
│   │   ├── Editor/
│   │   │   ├── DialogueEditor.cs
│   │   │   ├── CharacterEditor.cs
│   │   │   └── AudioEditor.cs
│   │   ├── Debug/
│   │   │   ├── DebugConsole.cs
│   │   │   ├── DebugUI.cs
│   │   │   └── DebugCommands.cs
│   │   └── Utilities/
│   │       ├── SceneUtilities.cs
│   │       ├── AssetUtilities.cs
│   │       └── BuildUtilities.cs
│   └── ThirdParty/                     # Integrações externas
│       ├── Analytics/
│       ├── Ads/
│       └── Social/
├── Prefabs/                           # Prefabs organizados
│   ├── UI/
│   │   ├── Menus/
│   │   ├── HUD/
│   │   └── Components/
│   ├── Characters/
│   │   ├── Sprites/
│   │   ├── Live2D/
│   │   └── Models/
│   ├── Audio/
│   │   ├── Music/
│   │   ├── SFX/
│   │   └── Voice/
│   └── Gameplay/
│       ├── Dialogue/
│       └── Scenes/
├── Materials/                          # Materiais organizados
│   ├── UI/
│   ├── Characters/
│   └── Effects/
├── Textures/                           # Texturas organizadas
│   ├── UI/
│   ├── Characters/
│   ├── Backgrounds/
│   └── Effects/
├── Audio/                              # Áudio organizado
│   ├── Music/
│   ├── SFX/
│   ├── Voice/
│   └── Ambient/
├── Animations/                         # Animações organizadas
│   ├── Characters/
│   ├── UI/
│   └── Effects/
└── Scenes/                             # Cenas organizadas
    ├── Core/
    │   ├── Bootstrap.unity
    │   ├── MainMenu.unity
    │   └── Loading.unity
    ├── Gameplay/
    │   ├── Chapter1/
    │   ├── Chapter2/
    │   └── Chapter3/
    └── Testing/
        ├── DialogueTest.unity
        └── AudioTest.unity
```

## Princípios da Estrutura AAA

### **1. Separação por Responsabilidade**
- **Core**: Sistemas fundamentais que nunca mudam
- **Gameplay**: Lógica específica do jogo
- **UI**: Interface do usuário
- **Audio**: Sistema de áudio
- **Data**: Dados e configurações

### **2. Hierarquia Clara**
```
Scripts/
├── Core/           # Fundação (Managers, Systems, Events)
├── Gameplay/       # Lógica do jogo (Dialogue, Characters, Scenes)
├── UI/            # Interface (Menus, HUD, Components)
├── Audio/         # Áudio (Music, SFX, Voice)
├── Input/         # Input (Keyboard, Gamepad, Touch)
├── Data/          # Dados (ScriptableObjects, Save, Localization)
├── Performance/   # Otimizações (Pooling, Memory, Profiling)
├── Tools/         # Desenvolvimento (Editor, Debug, Utilities)
└── ThirdParty/    # Integrações externas
```

### **3. Convenções de Nomenclatura**

#### **Scripts:**
- **Managers**: `[System]Manager.cs` (ex: `DialogueManager.cs`)
- **Controllers**: `[Feature]Controller.cs` (ex: `DialogueController.cs`)
- **Systems**: `[System]System.cs` (ex: `DialogueSystem.cs`)
- **Data**: `[Data]Data.cs` (ex: `DialogueData.cs`)
- **Events**: `[System]Events.cs` (ex: `DialogueEvents.cs`)

#### **Pastas:**
- **Singular**: Para sistemas únicos (`Manager/`, `System/`)
- **Plural**: Para múltiplas instâncias (`Characters/`, `Commands/`)

### **4. Organização de Assets**

#### **Prefabs por Funcionalidade:**
```
Prefabs/
├── UI/                    # Interface
├── Characters/            # Personagens
├── Audio/                # Áudio
└── Gameplay/             # Gameplay
```

#### **Materiais por Tipo:**
```
Materials/
├── UI/                   # Materiais de UI
├── Characters/           # Materiais de personagens
└── Effects/              # Materiais de efeitos
```

### **5. Estrutura de Cenas**

#### **Core Scenes:**
- `Bootstrap.unity` - Inicialização
- `MainMenu.unity` - Menu principal
- `Loading.unity` - Tela de carregamento

#### **Gameplay Scenes:**
- `Chapter1/` - Capítulo 1
- `Chapter2/` - Capítulo 2
- `Chapter3/` - Capítulo 3

#### **Testing Scenes:**
- `DialogueTest.unity` - Teste de diálogo
- `AudioTest.unity` - Teste de áudio

## Benefícios desta Estrutura

### **1. Escalabilidade**
- Fácil adicionar novos sistemas
- Estrutura preparada para crescimento
- Organização clara para equipes grandes

### **2. Manutenibilidade**
- Fácil localizar arquivos
- Separação clara de responsabilidades
- Código organizado e limpo

### **3. Performance**
- Carregamento otimizado
- Pooling de objetos
- Gerenciamento de memória

### **4. Colaboração**
- Estrutura intuitiva
- Convenções claras
- Fácil onboarding de novos desenvolvedores

### **5. Qualidade AAA**
- Padrões profissionais
- Organização enterprise-grade
- Preparado para produção

## Migração da Estrutura Atual

### **Fase 1: Reorganização (1-2 semanas)**
1. Criar nova estrutura de pastas
2. Mover scripts para pastas apropriadas
3. Atualizar namespaces
4. Testar se tudo funciona

### **Fase 2: Otimização (1-2 semanas)**
1. Reorganizar assets
2. Criar prefabs organizados
3. Otimizar cenas
4. Implementar convenções

### **Fase 3: Melhorias (1-2 semanas)**
1. Adicionar sistemas de performance
2. Implementar ferramentas de debug
3. Criar documentação
4. Treinar equipe

Esta estrutura transforma seu projeto em um padrão AAA profissional! 🚀

