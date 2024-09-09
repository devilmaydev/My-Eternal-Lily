using System;
using System.Collections.Generic;
using System.Text;

namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    public class CommandsData
    {
        public List<Command> Commands;
        private const char CommandSplitterId = ',';
        private const char ArgumentsContainerId = '(';
        private const string WaitCommandId = "[wait]";

        public CommandsData(string rawCommands)
        {
            Commands = RipRawCommands(rawCommands);
        }

        public List<Command> RipRawCommands(string rawCommands)
        {
            string[] commands = rawCommands.Split(CommandSplitterId, StringSplitOptions.RemoveEmptyEntries);

            var result = new List<Command>();

            foreach (var command in commands)
            {
                var cmd = new Command();
                
                int index = command.IndexOf(ArgumentsContainerId);
                cmd.Name = command.Substring(0, index).Trim();

                if (cmd.Name.ToLower().StartsWith(WaitCommandId))
                {
                    cmd.Name = cmd.Name.Substring(WaitCommandId.Length + 1);
                    cmd.WaitForCompletion = true;
                    
                }
                else
                    cmd.WaitForCompletion = false;
                
                cmd.Arguments = GetArgumments(command.Substring(index + 1, command.Length - index - 2));
                result.Add(cmd);
            }

            return result;
        }

        private string[] GetArgumments(string arguments)
        {
            var argList = new List<string>();
            var currentArg = new StringBuilder();
            var inQuotes = false;

            foreach (var t in arguments)
            {
                if (t == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (!inQuotes && t == ' ')
                {
                    argList.Add(currentArg.ToString());
                    currentArg.Clear();
                    continue;
                }

                currentArg.Append(t);
            }

            if (currentArg.Length > 0)
            {
                argList.Add(currentArg.ToString());
            }

            return argList.ToArray();
        }
    }

    public struct Command
    {
        public string Name;
        public string[] Arguments;
        public bool WaitForCompletion;
    }
}
