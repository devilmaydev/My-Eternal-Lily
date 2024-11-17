using System;
using System.Collections.Generic;

namespace _MAIN.Scripts.Core.Commands.Database
{
    public class CommandsDatabase
    {
        private Dictionary<string, Delegate> _database = new ();

        public bool HasCommand(string commandName) => _database.ContainsKey(commandName.ToLower());

        public void AddCommand(string commandName, Delegate command) => _database.TryAdd(commandName.ToLower(), command);
    
        public Delegate GetCommand(string commandName) => _database.GetValueOrDefault(commandName.ToLower());
    }
}
