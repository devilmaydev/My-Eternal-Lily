using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    private static Coroutine _process = null;
    private static bool _isRunning => _process != null;
    
    private CommandsDatabase _database;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _database = new CommandsDatabase();
            
            var assembly = Assembly.GetExecutingAssembly();

            var extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

            foreach (var extension in extensionTypes)
            {
                var extendMethod = extension.GetMethod("Extend");
                extendMethod.Invoke(null, new object[] { _database });
            }
        }
        else
            DestroyImmediate(gameObject);
    }

    public Coroutine Execute(string commandName, params string[] args)
    {
        var command = _database.GetCommand(commandName);

        if (command == null)
            return null;

        return StartProcess(commandName, command, args);
    }

    private Coroutine StartProcess(string commandName, Delegate command, string[] args)
    {
        StopCurrentProcess();

        return StartCoroutine(RunningProcess(command, args));
    }

    private IEnumerator RunningProcess(Delegate command, string[] args)
    {
        yield return WaitProcessComplete(command, args);
        
        _process = null;
    }

    private void StopCurrentProcess()
    {
        if (_process != null)
            StopCoroutine(_process);

        _process = null;
    }

    private IEnumerator WaitProcessComplete(Delegate command, string[] args)
    {
        switch (command)
        {
            case Action:
                command.DynamicInvoke();
                break;
            case Action<string>:
                command.DynamicInvoke(args[0]);
                break;
            case Action<string[]>:
                command.DynamicInvoke((object)args);
                break;
            case Func<IEnumerator> func:
                yield return func();
                break;
            case Func<string, IEnumerator> func:
                yield return func(args[0]);
                break;
            case Func<string[], IEnumerator> func:
                yield return func(args);
                break;
        }
    }
}
