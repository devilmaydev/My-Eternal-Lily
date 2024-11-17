using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Commands;
using _MAIN.Scripts.Core.Commands.Database;
using _MAIN.Scripts.Enums;
using _MAIN.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    private static Coroutine _process = null;
    private static bool _isRunning => _process != null;
    
    private CommandsDatabase _database;
    
    private Dictionary<string, CommandsDatabase> _subDatabases = new();
    public List<CommandProcess> ActiveProcesses = new();
    
    private const char SubCommandIdentifier = '.';
    public const string DatabaseCharactersBase = "characters";
    public const string DatabaseCharactersSprite = "characters_sprite";
    public const string DatabaseCharactersLive2D = "characters_live2D";
    public const string DatabaseCharactersModel3D = "characters_model3D";

    private CommandProcess TopProcess => ActiveProcesses.Last();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _database = new CommandsDatabase();
            
            var assembly = Assembly.GetExecutingAssembly();

            var extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CmdDatabaseExtension))).ToArray();

            foreach (var extension in extensionTypes)
            {
                var extendMethod = extension.GetMethod("Extend");
                if (extendMethod != null)
                    extendMethod.Invoke(null, new object[] { _database });
            }
        }
        else
            DestroyImmediate(gameObject);
    }
    
    private CoroutineWrapper ExecuteCharacterCommand(string commandName, params string[] args)
    {
        Delegate command;

        var db = _subDatabases[DatabaseCharactersBase];
        if (db.HasCommand(commandName))
        {
            command = db.GetCommand(commandName);
            return StartProcess(commandName, command, args);
        }

        var characterConfigData = CharacterManager.Instance.GetCharacterConfig(args[0]);
        
        db = characterConfigData.characterType switch
        {
            ECharacterType.Sprite or ECharacterType.SpriteSheet => _subDatabases[DatabaseCharactersSprite],
            ECharacterType.Live2D => _subDatabases[DatabaseCharactersLive2D],
            ECharacterType.Model3D => _subDatabases[DatabaseCharactersModel3D],
            _ => db
        };

        command = db.GetCommand(commandName);

        if (command != null)
            return StartProcess(commandName, command, args);

        Debug.LogError($"Command Manager was unable to execute command '{commandName}' on character '{args[0]}'. The character name or command may be invalid.");
        return null;
    }

    public CoroutineWrapper Execute(string commandName, params string[] args)
    {
        if (commandName.Contains(SubCommandIdentifier))
            return ExecuteSubCommand(commandName, args);
        var command = _database.GetCommand(commandName);
        
        return command == null ? null : StartProcess(commandName, command, args);
    }
    
    private CoroutineWrapper ExecuteSubCommand(string commandName, string[] args)
    {
        var parts = commandName.Split(SubCommandIdentifier);
        var databaseName = string.Join(SubCommandIdentifier, parts.Take(parts.Length - 1));
        var subCommandName = parts.Last();

        if (_subDatabases.ContainsKey(databaseName))
        {
            var command = _subDatabases[databaseName].GetCommand(subCommandName);
            if (command != null)
            {
                return StartProcess(commandName, command, args);
            }
            else
            {
                Debug.LogError($"No command called '{subCommandName}' was found in sub database '{databaseName}'");
                return null;
            }
        }

        string characterName = databaseName;
        //If we've made it here then we should try to run as a character command
        if (CharacterManager.Instance.HasCharacter(characterName))
        {
            var newArgs = new List<string>(args);
            newArgs.Insert(0, characterName);
            args = newArgs.ToArray();

            return ExecuteCharacterCommand(subCommandName, args);
        }

        Debug.LogError($"No sub database called '{databaseName}' exists! Command '{subCommandName}' could not be run.");
        return null;
    }

    private CoroutineWrapper StartProcess(string commandName, Delegate command, string[] args)
    {
        var processID = Guid.NewGuid();
        var cmd = new CommandProcess(processID, commandName, command, null, args, null);
        ActiveProcesses.Add(cmd);

        var co = StartCoroutine(RunningProcess(cmd));

        cmd.RunningProcess = new CoroutineWrapper(this, co);

        return cmd.RunningProcess;
    }

    private IEnumerator RunningProcess(CommandProcess process)
    {
        yield return WaitProcessComplete(process.Command, process.Args);

        KillProcess(process);
    }

    public void StopCurrentProcess()
    {
        if (TopProcess != null) 
            KillProcess(TopProcess);
    }
    
    public void StopAllProcesses()
    {
        foreach (var c in ActiveProcesses)
        {
            if (c.RunningProcess != null && !c.RunningProcess.IsDone)
                c.RunningProcess.Stop();

            c.OnTerminateAction?.Invoke();
        }

        ActiveProcesses.Clear();
    }
    
    public void KillProcess(CommandProcess cmd)
    {
        ActiveProcesses.Remove(cmd);

        if (cmd.RunningProcess is { IsDone: false })
            cmd.RunningProcess.Stop();

        cmd.OnTerminateAction?.Invoke();
    }
    
    private IEnumerator WaitProcessComplete(Delegate command, string[] args)
    {
        if (command is Action)
            command.DynamicInvoke();

        else if (command is Action<string>)
            command.DynamicInvoke(args.Length == 0 ? string.Empty : args[0]);

        else if (command is Action<string[]>)
            command.DynamicInvoke((object)args);

        else if (command is Func<IEnumerator>)
            yield return ((Func<IEnumerator>)command)();

        else if (command is Func<string, IEnumerator>)
            yield return ((Func<string, IEnumerator>)command)(args.Length == 0 ? string.Empty : args[0]);

        else if (command is Func<string[], IEnumerator>)
            yield return ((Func<string[], IEnumerator>)command)(args);
    }
    
    public void AddTerminationActionToCurrentProcess(UnityAction action)
    {
        var process = TopProcess;

        if (process == null)
            return;

        process.OnTerminateAction = new UnityEvent();
        process.OnTerminateAction.AddListener(action);
    }
    
    public CommandsDatabase CreateSubDatabase(string databaseName)
    {
        databaseName = databaseName.ToLower();

        if (_subDatabases.TryGetValue(databaseName, out CommandsDatabase db))
        {
            Debug.LogWarning($"A database by the name of '{databaseName}' already exists!");
            return db;
        }

        var newDatabase = new CommandsDatabase();
        _subDatabases.Add(databaseName, newDatabase);

        return newDatabase;
    }
}
