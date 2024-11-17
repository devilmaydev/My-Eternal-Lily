using System;
using System.Collections;
using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.Dialogue.Managers;
using _MAIN.Scripts.Core.IO;
using UnityEngine;

namespace _MAIN.Scripts.Core.Commands.Database.Extensions
{
    public class CmdDatabaseExtensionGeneral : CmdDatabaseExtension
    {
        private static readonly string[] ParamSpeed = { "-s", "-spd" };
        private static readonly string[] ParamImmediate = { "-i", "-immediate" };
        private static readonly string[] ParamFilepath = { "-f", "-file", "-filepath" };
        private static readonly string[] ParamEnqueue = { "-e", "-enqueue" };
        
        public new static void Extend(CommandsDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
            
            database.AddCommand("showui", new Func<string[], IEnumerator>(ShowDialogueSystem));
            database.AddCommand("hideui", new Func<string[], IEnumerator>(HideDialogueSystem));
            
            database.AddCommand("showdb", new Func<string[], IEnumerator>(ShowDialogueBox));
            database.AddCommand("hidedb", new Func<string[], IEnumerator>(HideDialogueBox));
            
            database.AddCommand("load", new Action<string[]>(LoadNewDialogueFile));
        }
        
        private static void LoadNewDialogueFile(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamFilepath, out string fileName);
            parameters.TryGetValue(ParamEnqueue, out bool enqueue, defaultValue: false);

            var filePath = FilePaths.GetPathToResource(FilePaths.ResourcesDialogueFiles, fileName);
            var file = Resources.Load<TextAsset>(filePath);

            if (file == null)
            {
                Debug.LogWarning($"File '{filePath}' could not be loaded from dialogue files. Please ensure it exists within the '{FilePaths.ResourcesDialogueFiles}' resources folder.");
                return;
            }

            var lines = FileManager.ReadTextAsset(file, includeBlankLines: true);
            var newConversation = new Conversation(lines);

            if (enqueue)
                DialogueSystem.Instance.ConversationManager.Enqueue(newConversation);
            else
                DialogueSystem.Instance.ConversationManager.StartConversation(newConversation);
        }

        private static IEnumerator Wait(string data)
        {
            if (float.TryParse(data, out float time))
                yield return new WaitForSeconds(time);
        }
        
        private static IEnumerator ShowDialogueBox(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamSpeed, out var speed, defaultValue: 1f);
            parameters.TryGetValue(ParamImmediate, out var immediate, defaultValue: false);

            yield return DialogueSystem.Instance.dialogueContainer.Show(speed, immediate);
        }

        private static IEnumerator HideDialogueBox(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamSpeed, out var speed, defaultValue: 1f);
            parameters.TryGetValue(ParamImmediate, out var immediate, defaultValue: false);

            yield return DialogueSystem.Instance.dialogueContainer.Hide(speed, immediate);
        }

        private static IEnumerator ShowDialogueSystem(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamSpeed, out var speed, defaultValue: 1f);
            parameters.TryGetValue(ParamImmediate, out var immediate, defaultValue: false);

            yield return DialogueSystem.Instance.Show(speed, immediate);
        }

        private static IEnumerator HideDialogueSystem(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamSpeed, out var speed, defaultValue: 1f);
            parameters.TryGetValue(ParamImmediate, out var immediate, defaultValue: false);

            yield return DialogueSystem.Instance.Hide(speed, immediate);
        }
    }
}