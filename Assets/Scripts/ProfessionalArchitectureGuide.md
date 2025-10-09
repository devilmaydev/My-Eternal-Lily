# 🏆 Guia de Arquitetura Profissional

## 📋 **Estrutura do Projeto:**

```
Assets/Scripts/
├── Core/
│   ├── Managers/
│   │   ├── Interfaces/
│   │   │   └── IGameManager.cs
│   │   └── GameManager.cs
│   ├── Systems/
│   │   ├── Audio/
│   │   │   ├── Interfaces/
│   │   │   ├── Events/
│   │   │   ├── Settings/
│   │   │   └── AudioManager.cs
│   │   └── Input/
│   │       ├── Interfaces/
│   │       ├── Events/
│   │       ├── Settings/
│   │       └── InputManager.cs
│   ├── Dialogue/
│   ├── Characters/
│   └── Commands/
└── UI/
```

## 🎮 **GameManager - Sistema Central**

### **Configuração Manual (Padrão da Indústria):**

1. **Crie um GameObject** chamado "GameManager"
2. **Adicione o script** `GameManager`
3. **Configure as referências** no Inspector:
   - AudioManager
   - InputManager
   - CharacterManager
   - DialogueSystem
   - CommandManager

### **Uso:**
```csharp
// Acesso aos managers
var audioManager = GameManager.Instance.AudioManager;
var inputManager = GameManager.Instance.InputManager;

// Controle do jogo
GameManager.Instance.InitializeGame();
GameManager.Instance.StartGame();
GameManager.Instance.PauseGame();
```

## 🎵 **Audio System**

### **Configuração:**
1. **Crie um GameObject** "AudioManager"
2. **Adicione o script** `AudioManager`
3. **Configure** `VnAudioSettings` no Inspector
4. **Atribua** AudioMixerGroups

### **Uso:**
```csharp
// Tocar som
AudioManager.Instance.PlaySoundEffect("Audio/SFX/button_click");

// Tocar música
AudioManager.Instance.PlayTrack("Audio/Music/theme", channel: 0);
```

## ⌨️ **Input System**

### **Configuração:**
1. **Crie um GameObject** "InputManager"
2. **Adicione o script** `InputManager`
3. **Configure** `VnInputSettings` no Inspector
4. **Atribua** InputActionAsset

### **Uso:**
```csharp
// Verificar input
bool isSpacePressed = InputManager.Instance.IsKeyDown(KeyCode.Space);

// Controlar input
InputManager.Instance.EnableInput();
InputManager.Instance.DisableInput();
```

## 💬 **Dialogue System**

### **Configuração:**
1. **Crie um GameObject** "DialogueSystem"
2. **Adicione o script** `DialogueSystem`
3. **Configure** `VnDialogueSettings` no Inspector
4. **Atribua** CanvasGroup e configurações

### **Uso:**
```csharp
// Falar diálogo
DialogueSystem.Instance.Say("Character", "Hello World!");

// Controlar UI
DialogueSystem.Instance.Show();
DialogueSystem.Instance.Hide();

// Gerenciar speaker
DialogueSystem.Instance.ShowSpeakerName("Character");
DialogueSystem.Instance.HideSpeakerName();
```

## 🎮 **Command System**

### **Configuração:**
1. **Crie um GameObject** "CommandManager"
2. **Adicione o script** `CommandManager`
3. **Configure** `VnCommandSettings` no Inspector
4. **Configure** sub-databases conforme necessário

### **Uso:**
```csharp
// Executar comando
CommandManager.Instance.Execute("load", "-f", "A1C1");

// Executar sub-comando
CommandManager.Instance.ExecuteSubCommand("characters.show", "CharacterName");

// Executar comando de personagem
CommandManager.Instance.ExecuteCharacterCommand("show", "CharacterName");

// Gerenciar processos
CommandManager.Instance.StopCurrentProcess();
CommandManager.Instance.StopAllProcesses();
```

## 👥 **Character System**

### **Configuração:**
1. **Crie um GameObject** "CharacterManager"
2. **Adicione o script** `CharacterManager`
3. **Configure** `VnCharacterSettings` no Inspector
4. **Atribua** CharacterPanel e configurações

### **Uso:**
```csharp
// Criar personagem
CharacterManager.Instance.CreateCharacter("CharacterName", revealAfterCreated: true);

// Obter personagem
var character = CharacterManager.Instance.GetCharacter("CharacterName");

// Verificar se existe
bool hasCharacter = CharacterManager.Instance.HasCharacter("CharacterName");

// Ordenar personagens
CharacterManager.Instance.SortCharacters();
CharacterManager.Instance.SortCharacters(new[] {"Char1", "Char2"});

// Destruir personagem
CharacterManager.Instance.DestroyCharacter("CharacterName");
```

## 🔗 **Event System**

### **Uso:**
```csharp
// Se inscrever em eventos
InputEvents.OnNextPressed += OnNextPressed;
AudioEvents.OnSoundEffectPlayed += OnSoundPlayed;
DialogueEvents.OnDialogueStarted += OnDialogueStarted;
DialogueEvents.OnSpeakerChanged += OnSpeakerChanged;
CommandEvents.OnCommandExecuted += OnCommandExecuted;
CommandEvents.OnProcessStarted += OnProcessStarted;
CharacterEvents.OnCharacterCreated += OnCharacterCreated;
CharacterEvents.OnCharacterShown += OnCharacterShown;

// Não esquecer de se desinscrever
InputEvents.OnNextPressed -= OnNextPressed;
AudioEvents.OnSoundEffectPlayed -= OnSoundPlayed;
DialogueEvents.OnDialogueStarted -= OnDialogueStarted;
DialogueEvents.OnSpeakerChanged -= OnSpeakerChanged;
CommandEvents.OnCommandExecuted -= OnCommandExecuted;
CommandEvents.OnProcessStarted -= OnProcessStarted;
CharacterEvents.OnCharacterCreated -= OnCharacterCreated;
CharacterEvents.OnCharacterShown -= OnCharacterShown;
```

### **Separação de Responsabilidades:**
- **InputEvents.OnNextPressed** - Evento de input (Space, Enter, etc.)
- **DialogueEvents.OnDialogueStarted** - Evento de diálogo iniciado
- **DialogueEvents.OnSpeakerChanged** - Evento de mudança de speaker
- **AudioEvents.OnSoundEffectPlayed** - Evento de som tocado
- **CommandEvents.OnCommandExecuted** - Evento de comando executado
- **CommandEvents.OnProcessStarted** - Evento de processo iniciado
- **CharacterEvents.OnCharacterCreated** - Evento de personagem criado
- **CharacterEvents.OnCharacterShown** - Evento de personagem mostrado

## 📝 **Boas Práticas:**

### **✅ DO (Faça):**
- Configure managers manualmente no Inspector
- Use interfaces para contratos claros
- Implemente event system para desacoplamento
- Documente código com XML comments
- Use namespaces organizados
- Mantenha responsabilidades separadas

### **❌ DON'T (Não faça):**
- Não use setup automático
- Não crie scripts de "mágica"
- Não acople sistemas desnecessariamente
- Não use singletons excessivamente
- Não ignore tratamento de erros

## 🎯 **Padrões da Indústria:**

### **1. Manager Pattern:**
- Um manager por sistema
- Responsabilidades claras
- Interface bem definida

### **2. Event System:**
- Comunicação desacoplada
- Fácil extensão
- Testabilidade

### **3. Singleton Pattern:**
- Acesso global controlado
- Uma instância por sistema
- Inicialização segura

### **4. Interface Segregation:**
- Contratos claros
- Fácil mock para testes
- Flexibilidade

## 🚀 **Próximos Passos:**

1. **Configure** os managers manualmente
2. **Teste** cada sistema individualmente
3. **Integre** os sistemas via GameManager
4. **Documente** qualquer customização

## 🏆 **Resultado:**

- **Código limpo** e profissional
- **Arquitetura** escalável
- **Padrões** da indústria
- **Fácil manutenção** e extensão
- **Testabilidade** alta

**Sistema profissional e pronto para produção!** 🎯
