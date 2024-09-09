using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandsDatabase
{
    private Dictionary<string, Delegate> _database = new ();

    public bool HasCommand(string commandName) 
        => _database.ContainsKey(commandName);

    public void AddCommand(string commandName, Delegate command) 
        => _database.TryAdd(commandName, command);
    
    public Delegate GetCommand(string commandName) 
        => _database.GetValueOrDefault(commandName);
}
