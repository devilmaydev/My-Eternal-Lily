using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.Dialogue.DataContainers;
using _MAIN.Scripts.Core.IO;
using UnityEngine;

namespace _TESTING.Scripts
{
    public class TestConversations : MonoBehaviour
    {
        private void Start()
        {
            StartConversation();
        }

        void StartConversation()
        {
            var lines = FileManager.ReadTextAsset("testFile 1");

            // foreach (var line in lines)
            // {
            //     if (string.IsNullOrWhiteSpace(line))
            //         continue;
            //
            //     var dl = DialogueParser.Parse(line);
            //
            //     for (int i = 0; i < dl.CommandsData.Commands.Count; i++)
            //     {
            //         var command = dl.CommandsData.Commands[i];
            //         Debug.Log($"Command [{i}] '{command.Name}' has arguments [{string.Join(", ", command.Arguments)}]");
            //     }
            // }
            
            DialogueSystem.Instance.Say(lines);
        }
    }
}