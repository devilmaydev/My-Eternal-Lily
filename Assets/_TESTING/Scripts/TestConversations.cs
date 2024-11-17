using System.IO;
using _MAIN.Scripts.Core.Dialogue;
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
            var lines = FileManager.ReadTextAsset(FilePaths.ResourcesDialogueFiles + "Ato1");
            DialogueSystem.Instance.Say(lines);
        }
    }
}