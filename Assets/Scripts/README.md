# 🎮 Visual Novel Engine - Scripts

## 📋 **Visão Geral**

Sistema de scripts profissional para Visual Novel Engine, seguindo padrões AAA da indústria de jogos.

## 🏗️ **Arquitetura**

### **Core Systems:**
- **GameManager** - Gerenciador central de todos os sistemas
- **AudioManager** - Sistema de áudio com pooling e eventos
- **InputManager** - Sistema de input moderno com Input System
- **CharacterManager** - Gerenciamento de personagens
- **DialogueSystem** - Sistema de diálogo
- **CommandManager** - Sistema de comandos

### **Padrões Utilizados:**
- **Singleton Pattern** - Acesso global controlado
- **Manager Pattern** - Um manager por sistema
- **Event System** - Comunicação desacoplada
- **Interface Segregation** - Contratos claros
- **Dependency Injection** - Injeção de dependências

## 🚀 **Configuração Rápida**

### **1. GameManager:**
```csharp
// Crie um GameObject "GameManager"
// Adicione o script GameManager
// Configure as referências no Inspector
```

### **2. Audio System:**
```csharp
// Crie um GameObject "AudioManager"
// Adicione o script AudioManager
// Configure VnAudioSettings
```

### **3. Input System:**
```csharp
// Crie um GameObject "InputManager"
// Adicione o script InputManager
// Configure VnInputSettings
```

## 📚 **Documentação**

- [ProfessionalArchitectureGuide.md](./ProfessionalArchitectureGuide.md) - Guia completo de arquitetura
- [AudioSystemRefactoringSummary.md](./AudioSystemRefactoringSummary.md) - Detalhes do sistema de áudio

## 🎯 **Características**

- ✅ **Código limpo** e profissional
- ✅ **Arquitetura escalável**
- ✅ **Padrões da indústria**
- ✅ **Fácil manutenção**
- ✅ **Alta testabilidade**
- ✅ **Documentação completa**

## 🔧 **Desenvolvimento**

### **Estrutura de Pastas:**
```
Core/
├── Managers/          # Gerenciadores centrais
├── Systems/           # Sistemas específicos
├── Dialogue/          # Sistema de diálogo
├── Characters/        # Sistema de personagens
└── Commands/          # Sistema de comandos
```

### **Convenções:**
- **Interfaces** em pastas `Interfaces/`
- **Events** em pastas `Events/`
- **Settings** em pastas `Settings/`
- **Namespaces** organizados por funcionalidade

## 🏆 **Qualidade**

- **Sem setup automático** - Configuração manual profissional
- **Sem scripts de "mágica"** - Código transparente
- **Sem acoplamento desnecessário** - Sistemas independentes
- **Com documentação clara** - Fácil de entender e manter

**Sistema profissional pronto para produção!** 🎯
