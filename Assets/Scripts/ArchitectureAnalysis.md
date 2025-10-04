# Análise Detalhada da Nova Arquitetura - Visual Novel Engine

## Visão Geral da Arquitetura

A nova arquitetura segue os princípios da **Clean Architecture** com **CQRS** e **Event-Driven Architecture**, organizando o código em 4 camadas principais:

```
┌─────────────────────────────────────────────────────────┐
│                 PRESENTATION LAYER                     │
│  ┌─────────────────┐  ┌─────────────────┐             │
│  │   UI Controllers │  │  Input Handlers │             │
│  │   ViewModels    │  │  Event Handlers │             │
│  └─────────────────┘  └─────────────────┘             │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│                 APPLICATION LAYER                     │
│  ┌─────────────────┐  ┌─────────────────┐             │
│  │  Command Handlers│  │  Query Handlers  │             │
│  │  Event Handlers │  │  Use Cases      │             │
│  │  Services       │  │  Orchestrators  │             │
│  └─────────────────┘  └─────────────────┘             │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│                    DOMAIN LAYER                         │
│  ┌─────────────────┐  ┌─────────────────┐             │
│  │   Entities      │  │   Value Objects │             │
│  │   Services      │  │   Interfaces    │             │
│  │   Events        │  │   Specifications│             │
│  └─────────────────┘  └─────────────────┘             │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│               INFRASTRUCTURE LAYER                     │
│  ┌─────────────────┐  ┌─────────────────┐             │
│  │   Repositories  │  │   External APIs │             │
│  │   Data Access   │  │   File System   │             │
│  │   Services      │  │   Persistence   │             │
│  └─────────────────┘  └─────────────────┘             │
└─────────────────────────────────────────────────────────┘
```

## 1. DOMAIN LAYER - Regras de Negócio

### Entidades Principais

#### Dialogue (Entidade)
```csharp
public class Dialogue
{
    public string Id { get; private set; }
    public string Title { get; private set; }
    public List<DialogueLine> Lines { get; private set; }
    public DialogueStatus Status { get; private set; }
    
    // Métodos de negócio
    public void Start() { /* Regras para iniciar diálogo */ }
    public void Complete() { /* Regras para finalizar diálogo */ }
    public bool CanContinue() { /* Regras de continuidade */ }
}
```

#### Character (Entidade)
```csharp
public class Character
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public CharacterType Type { get; private set; }
    public CharacterState State { get; private set; }
    
    // Métodos de negócio
    public void Show() { /* Regras para mostrar personagem */ }
    public void Hide() { /* Regras para esconder personagem */ }
    public void SetPosition(Position position) { /* Regras de posicionamento */ }
}
```

### Value Objects
```csharp
public class DialogueLine
{
    public string Text { get; private set; }
    public string Speaker { get; private set; }
    public List<Command> Commands { get; private set; }
}

public class Position
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }
}
```

### Domain Events
```csharp
public class DialogueStartedEvent : IDomainEvent
{
    public string DialogueId { get; set; }
    public DateTime Timestamp { get; set; }
}

public class CharacterShownEvent : IDomainEvent
{
    public string CharacterId { get; set; }
    public string CharacterName { get; set; }
}
```

## 2. APPLICATION LAYER - Casos de Uso

### Commands (Ações que modificam estado)

#### StartDialogueCommand
```csharp
public class StartDialogueCommand : ICommand
{
    public string DialogueId { get; set; }
    public string CharacterName { get; set; }
}

public class StartDialogueCommandHandler : ICommandHandler<StartDialogueCommand, CommandResult>
{
    private readonly IDialogueRepository _dialogueRepository;
    private readonly ICharacterService _characterService;
    private readonly IEventBus _eventBus;
    
    public async Task<CommandResult> HandleAsync(StartDialogueCommand command)
    {
        // 1. Buscar diálogo
        var dialogue = await _dialogueRepository.GetByIdAsync(command.DialogueId);
        
        // 2. Aplicar regras de negócio
        dialogue.Start();
        
        // 3. Persistir mudanças
        await _dialogueRepository.SaveAsync(dialogue);
        
        // 4. Publicar eventos
        await _eventBus.PublishAsync(new DialogueStartedEvent(dialogue.Id));
        
        return new CommandResult { Success = true };
    }
}
```

#### ShowCharacterCommand
```csharp
public class ShowCharacterCommand : ICommand
{
    public string CharacterId { get; set; }
    public Position Position { get; set; }
    public float Speed { get; set; } = 1f;
}
```

### Queries (Leitura de dados)

#### GetDialogueQuery
```csharp
public class GetDialogueQuery : IQuery<DialogueDto>
{
    public string DialogueId { get; set; }
}

public class GetDialogueQueryHandler : IQueryHandler<GetDialogueQuery, DialogueDto>
{
    private readonly IDialogueRepository _dialogueRepository;
    
    public async Task<DialogueDto> HandleAsync(GetDialogueQuery query)
    {
        var dialogue = await _dialogueRepository.GetByIdAsync(query.DialogueId);
        return new DialogueDto
        {
            Id = dialogue.Id,
            Title = dialogue.Title,
            Status = dialogue.Status.ToString(),
            LineCount = dialogue.Lines.Count
        };
    }
}
```

### Event Handlers

#### DialogueStartedEventHandler
```csharp
public class DialogueStartedEventHandler : IEventHandler<DialogueStartedEvent>
{
    private readonly IAudioService _audioService;
    private readonly IUIService _uiService;
    
    public async Task HandleAsync(DialogueStartedEvent domainEvent)
    {
        // Tocar música de fundo
        await _audioService.PlayBackgroundMusicAsync("dialogue_music");
        
        // Mostrar UI do diálogo
        await _uiService.ShowDialogueUIAsync();
    }
}
```

## 3. INFRASTRUCTURE LAYER - Implementações

### Repositories

#### DialogueRepository
```csharp
public class DialogueRepository : IDialogueRepository
{
    private readonly IDataContext _dataContext;
    
    public async Task<Dialogue> GetByIdAsync(string id)
    {
        // Implementação específica para Unity (Resources, Addressables, etc.)
        var data = await _dataContext.LoadDialogueDataAsync(id);
        return MapToEntity(data);
    }
    
    public async Task SaveAsync(Dialogue dialogue)
    {
        var data = MapToData(dialogue);
        await _dataContext.SaveDialogueDataAsync(data);
    }
}
```

### Services

#### AudioService
```csharp
public class AudioService : IAudioService
{
    private readonly AudioManager _audioManager;
    
    public async Task PlaySoundEffectAsync(string soundPath)
    {
        _audioManager.PlaySoundEffect(soundPath);
    }
    
    public async Task PlayBackgroundMusicAsync(string musicPath)
    {
        _audioManager.PlayTrack(musicPath);
    }
}
```

## 4. PRESENTATION LAYER - UI e Input

### Controllers

#### DialogueController
```csharp
public class DialogueController : MonoBehaviour
{
    private ICommandBus _commandBus;
    private IQueryBus _queryBus;
    private IEventBus _eventBus;
    
    [SerializeField] private Button continueButton;
    [SerializeField] private Text dialogueText;
    
    private void Start()
    {
        // Injeção de dependências via VContainer
        _commandBus = Container.Resolve<ICommandBus>();
        _queryBus = Container.Resolve<IQueryBus>();
        _eventBus = Container.Resolve<IEventBus>();
        
        // Configurar eventos
        continueButton.onClick.AddListener(OnContinueClicked);
        _eventBus.Subscribe<DialogueLineChangedEvent>(OnDialogueLineChanged);
    }
    
    private async void OnContinueClicked()
    {
        var command = new ContinueDialogueCommand();
        await _commandBus.SendAsync(command);
    }
    
    private void OnDialogueLineChanged(DialogueLineChangedEvent evt)
    {
        dialogueText.text = evt.LineText;
    }
}
```

### Input Handlers

#### InputHandler
```csharp
public class InputHandler : MonoBehaviour
{
    private ICommandBus _commandBus;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var command = new ContinueDialogueCommand();
            _commandBus.SendAsync(command);
        }
    }
}
```

## 5. DEPENDENCY INJECTION - VContainer

### Container Configuration
```csharp
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Domain Services
        Container.Bind<IDialogueService>().To<DialogueService>().AsSingle();
        Container.Bind<ICharacterService>().To<CharacterService>().AsSingle();
        
        // Application Services
        Container.Bind<ICommandBus>().To<CommandBus>().AsSingle();
        Container.Bind<IQueryBus>().To<QueryBus>().AsSingle();
        Container.Bind<IEventBus>().To<EventBus>().AsSingle();
        
        // Infrastructure
        Container.Bind<IDialogueRepository>().To<DialogueRepository>().AsSingle();
        Container.Bind<IAudioService>().To<AudioService>().AsSingle();
        Container.Bind<IUIService>().To<UIService>().AsSingle();
        
        // Command Handlers
        Container.Bind<ICommandHandler<StartDialogueCommand, CommandResult>>()
            .To<StartDialogueCommandHandler>().AsTransient();
        
        // Query Handlers
        Container.Bind<IQueryHandler<GetDialogueQuery, DialogueDto>>()
            .To<GetDialogueQueryHandler>().AsTransient();
    }
}
```

## 6. FLUXO COMPLETO - Exemplo Prático

### Cenário: Iniciar um diálogo

1. **Input** → Usuário clica em "Iniciar Diálogo"
2. **Presentation** → `DialogueController.OnStartDialogueClicked()`
3. **Application** → `StartDialogueCommand` é criado
4. **Application** → `StartDialogueCommandHandler` processa o comando
5. **Domain** → `Dialogue.Start()` aplica regras de negócio
6. **Infrastructure** → `DialogueRepository.SaveAsync()` persiste dados
7. **Application** → `DialogueStartedEvent` é publicado
8. **Presentation** → `DialogueStartedEventHandler` reage ao evento
9. **Infrastructure** → `AudioService` toca música
10. **Presentation** → UI é atualizada

## 7. BENEFÍCIOS DA NOVA ARQUITETURA

### Testabilidade
```csharp
[Test]
public async Task StartDialogue_ShouldPublishEvent()
{
    // Arrange
    var mockRepository = new Mock<IDialogueRepository>();
    var mockEventBus = new Mock<IEventBus>();
    var handler = new StartDialogueCommandHandler(mockRepository.Object, mockEventBus.Object);
    
    // Act
    await handler.HandleAsync(new StartDialogueCommand { DialogueId = "test" });
    
    // Assert
    mockEventBus.Verify(x => x.PublishAsync(It.IsAny<DialogueStartedEvent>()), Times.Once);
}
```

### Manutenibilidade
- Cada classe tem uma responsabilidade única
- Fácil localizar e modificar funcionalidades
- Código auto-documentado

### Escalabilidade
- Fácil adicionar novos comandos/queries
- Novos sistemas podem se integrar via eventos
- Arquitetura preparada para crescimento

### Performance
- Lazy loading de dependências
- Event-driven para comunicação assíncrona
- Separação clara entre leitura e escrita

## 8. MIGRAÇÃO GRADUAL

### Fase 1: Preparação
- Instalar VContainer
- Criar estrutura de pastas
- Configurar testes

### Fase 2: Domain Layer
- Extrair entidades do código atual
- Criar value objects
- Definir domain events

### Fase 3: Application Layer
- Criar commands para ações existentes
- Implementar command handlers
- Adicionar queries para leitura

### Fase 4: Infrastructure
- Implementar repositories
- Criar services concretos
- Configurar DI container

### Fase 5: Presentation
- Refatorar controllers
- Implementar event handlers
- Conectar UI com nova arquitetura

## 9. COMPARAÇÃO: ANTES vs DEPOIS

### Antes (Singleton Pattern)
```csharp
// Uso direto e acoplado
DialogueSystem.Instance.Say("Hello World");
CharacterManager.Instance.ShowCharacter("Alice");
AudioManager.Instance.PlaySound("click");
```

### Depois (Clean Architecture)
```csharp
// Uso via Command Bus
await _commandBus.SendAsync(new SayDialogueCommand { Text = "Hello World" });
await _commandBus.SendAsync(new ShowCharacterCommand { CharacterId = "Alice" });
await _commandBus.SendAsync(new PlaySoundCommand { SoundPath = "click" });
```

### Vantagens do "Depois":
- ✅ Testável (fácil mockar CommandBus)
- ✅ Flexível (fácil trocar implementações)
- ✅ Rastreável (logs centralizados)
- ✅ Extensível (novos comandos sem modificar código existente)
- ✅ Assíncrono (não bloqueia UI)

Esta arquitetura transforma seu Visual Novel Engine em uma solução enterprise-grade, mantendo a simplicidade de uso mas com robustez para projetos grandes! 🚀

