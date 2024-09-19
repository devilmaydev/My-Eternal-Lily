using System.Collections.Generic;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Dialogue.DataContainers;
using _MAIN.Scripts.Core.Dialogue.Managers;
using _MAIN.Scripts.Core.ScriptableObjects;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance { get; private set; }
        
        [SerializeField] private DialogueSystemConfigurationSO config;
        public DialogueSystemConfigurationSO Config => config;
        
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

            _textArchitect = new TextArchitect(dialogueContainer.dialogueText);
            _conversationManager = new ConversationManager(_textArchitect);
            _isInitialized = true;
        }
        
        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            Character character = CharacterManager.Instance.GetCharacter(speakerName);
            CharacterConfigData characterConfigData = character != null ? character.Config : CharacterManager.Instance.GetCharacterConfig(speakerName);

            ApplySpeakerDataToDialogueContainer(characterConfigData);
        }

        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData configuration)
        {
            dialogueContainer.SetDialogueColor(configuration.dialogueColor);
            dialogueContainer.SetDialogueFont(configuration.dialogueFont);
            dialogueContainer.nameContainer.SetNameColor(configuration.nameColor);
            dialogueContainer.nameContainer.SetNameFont(configuration.nameFont);
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator")
                dialogueContainer.nameContainer.Show(speakerName);
            else
                HideSpeakerName();
        }

        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        public Coroutine Say(string speaker, string dialogue)
        {
            var conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> conversation)
        {
            return _conversationManager.StartConversation(conversation);
        }

        public void OnUserPromptNext()
        {
            OnUserPromptNextEvent?.Invoke();
        }
    }
}
