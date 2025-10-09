using Core.Commands;

namespace Core.Systems.Commands.Events
{
    /// <summary>
    /// Eventos centralizados do sistema de comandos
    /// Permite comunicação desacoplada entre sistemas
    /// </summary>
    public static class CommandEvents
    {
        // Command Events
        public static event System.Action<string> OnCommandExecuted;
        public static event System.Action<string> OnCommandFailed;
        public static event System.Action<string, string[]> OnCommandStarted;
        public static event System.Action<string, string[]> OnCommandCompleted;
        
        // Process Events
        public static event System.Action<CommandProcess> OnProcessStarted;
        public static event System.Action<CommandProcess> OnProcessCompleted;
        public static event System.Action<CommandProcess> OnProcessStopped;
        public static event System.Action<CommandProcess> OnProcessKilled;
        
        // Database Events
        public static event System.Action<string> OnSubDatabaseCreated;
        public static event System.Action<string, string> OnCommandRegistered;
        public static event System.Action<string> OnCommandNotFound;
        
        // Character Command Events
        public static event System.Action<string, string> OnCharacterCommandExecuted;
        public static event System.Action<string, string> OnCharacterCommandFailed;
        
        // Event Invokers
        public static void InvokeCommandExecuted(string commandName) => OnCommandExecuted?.Invoke(commandName);
        public static void InvokeCommandFailed(string commandName) => OnCommandFailed?.Invoke(commandName);
        public static void InvokeCommandStarted(string commandName, string[] args) => OnCommandStarted?.Invoke(commandName, args);
        public static void InvokeCommandCompleted(string commandName, string[] args) => OnCommandCompleted?.Invoke(commandName, args);
        
        public static void InvokeProcessStarted(CommandProcess process) => OnProcessStarted?.Invoke(process);
        public static void InvokeProcessCompleted(CommandProcess process) => OnProcessCompleted?.Invoke(process);
        public static void InvokeProcessStopped(CommandProcess process) => OnProcessStopped?.Invoke(process);
        public static void InvokeProcessKilled(CommandProcess process) => OnProcessKilled?.Invoke(process);
        
        public static void InvokeSubDatabaseCreated(string databaseName) => OnSubDatabaseCreated?.Invoke(databaseName);
        public static void InvokeCommandRegistered(string databaseName, string commandName) => OnCommandRegistered?.Invoke(databaseName, commandName);
        public static void InvokeCommandNotFound(string commandName) => OnCommandNotFound?.Invoke(commandName);
        
        public static void InvokeCharacterCommandExecuted(string characterName, string commandName) => OnCharacterCommandExecuted?.Invoke(characterName, commandName);
        public static void InvokeCharacterCommandFailed(string characterName, string commandName) => OnCharacterCommandFailed?.Invoke(characterName, commandName);
    }
}
