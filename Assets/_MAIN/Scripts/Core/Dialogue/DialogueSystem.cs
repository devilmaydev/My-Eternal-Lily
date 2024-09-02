using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue.DataContainers;
using _MAIN.Scripts.Core.Dialogue.Managers;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance { get; private set; }
        
        public DialogueContainer dialogueContainer = new();
        private ConversationManager _conversationManager;
        private TextArchitect _textArchitect;

        public bool IsRunningConversation => _conversationManager.IsRunning;
        private bool _isInitialized;

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent OnUserPromptNextEvent;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }
            else
                DestroyImmediate(gameObject);
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;

            _textArchitect = new TextArchitect(dialogueContainer.DialogueText);
            _conversationManager = new ConversationManager(_textArchitect);
            _isInitialized = true;
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator")
                dialogueContainer.SpeakerText.ShowSpeaker(speakerName);
            else
                HideSpeakerName();
        }

        public void HideSpeakerName() => dialogueContainer.SpeakerText.HideSpeaker();

        public void Say(string speaker, string dialogue)
        {
            var conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            Say(conversation);
        }

        public void Say(List<string> conversation)
        {
            _conversationManager.StartConversation(conversation);
        }

        public void OnUserPromptNext()
        {
            OnUserPromptNextEvent?.Invoke();
        }
    }
}
