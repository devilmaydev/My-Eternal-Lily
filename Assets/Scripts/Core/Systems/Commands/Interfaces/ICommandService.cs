using System;
using System.Collections;
using Core.Commands;
using UnityEngine;
using UnityEngine.Events;
using Core.Commands.Database;
using Core.Utils.Extensions;

namespace Core.Systems.Commands.Interfaces
{
    /// <summary>
    /// Interface para o sistema de comandos
    /// Define contratos para execução e gerenciamento de comandos
    /// </summary>
    public interface ICommandService
    {
        // Events
        event System.Action<string> OnCommandExecuted;
        event System.Action<string> OnCommandFailed;
        event System.Action<CommandProcess> OnProcessStarted;
        event System.Action<CommandProcess> OnProcessCompleted;
        event System.Action<CommandProcess> OnProcessStopped;
        
        // Command State
        bool IsRunning { get; }
        int ActiveProcessCount { get; }
        
        // Command Execution
        CoroutineWrapper Execute(string commandName, params string[] args);
        CoroutineWrapper ExecuteSubCommand(string commandName, string[] args);
        
        // Process Management
        void StopCurrentProcess();
        void StopAllProcesses();
        void KillProcess(CommandProcess process);
        
        // Database Management
        CommandsDatabase CreateSubDatabase(string databaseName);
        bool HasCommand(string commandName);
        bool HasSubCommand(string databaseName, string commandName);
        
        // Process Actions
        void AddTerminationActionToCurrentProcess(UnityAction action);
        
        // Character Commands
        CoroutineWrapper ExecuteCharacterCommand(string commandName, params string[] args);
    }
}
