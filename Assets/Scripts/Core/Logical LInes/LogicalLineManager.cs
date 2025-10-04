using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _MAIN.Scripts.Core.Logical_LInes;
using Core.Dialogue;
using Core.Dialogue.DataContainers;
using UnityEngine;

namespace Core.Logical_LInes
{
    public class LogicalLineManager
    {
        private DialogueSystem DialogueSystem => DialogueSystem.Instance;
        private List<ILogicalLine> _logicalLines = new();

        public LogicalLineManager() => LoadLogicalLines();

        private void LoadLogicalLines()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var lineTypes = assembly.GetTypes()
                                          .Where(t => typeof(ILogicalLine)
                                          .IsAssignableFrom(t) && !t.IsInterface)
                                          .ToArray();

            foreach (var lineType in lineTypes)
            {
                var line = (ILogicalLine)Activator.CreateInstance(lineType);
                _logicalLines.Add(line);
            }
        }
        public bool TryGetLogic(DialogueLine line, out Coroutine logic)
        {
            foreach (var logicalLine in _logicalLines)
            {
                if (logicalLine.Matches(line))
                {
                    logic = DialogueSystem.StartCoroutine(logicalLine.Execute(line));
                    return true;
                }
            }
            
            logic = null;
            return false;
        }
    }
}
