using System;
using _MAIN.Scripts.Extensions;
using UnityEngine.Events;

namespace _MAIN.Scripts.Core.Commands
{
    public class CommandProcess
    {
        public Guid ID;
        public string ProcessName;
        public Delegate Command;
        public CoroutineWrapper RunningProcess;
        public string[] Args;

        public UnityEvent OnTerminateAction;

        public CommandProcess(Guid iD, string processName, Delegate command, CoroutineWrapper runningProcess, string[] args, UnityEvent onTerminateAction = null)
        {
            ID = iD;
            ProcessName = processName;
            Command = command;
            RunningProcess = runningProcess;
            Args = args;
            OnTerminateAction = onTerminateAction;
        }
    }
}