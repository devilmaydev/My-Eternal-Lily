using System.Collections.Generic;
using _MAIN.Scripts.Core.ScriptableObjects;
using Core.Characters;
using Core.Dialogue;
using Core.Dialogue.DataContainers;
using Core.Dialogue.Managers;
using Core.Systems.Dialogue.Events;
using Core.Systems.Dialogue.Interfaces;
using Core.Systems.Dialogue.Settings;
using Core.Managers;
using UnityEngine;

namespace Core.Systems.Dialogue
{
    public class DialogueSystem : MonoBehaviour, IDialogueService
    {
        public static DialogueSystem Instance { get; private set; }
        
        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private DialogueSystemConfigurationSO config;
        [SerializeField] private VnDialogueSettings dialogueSettings;
        public DialogueSystemConfigurationSO Config => config;
        public VnDialogueSettings DialogueSettings => dialogueSettings;
        
        public DialogueContainer dialogueContainer = new();
        public DialogueContinuePrompt dialogueContinuePrompt;
        public ConversationManager ConversationManager { get; private set; }
        private TextArchitect _textArchitect;
        private AutoReader _autoReader;
        private CanvasGroupController _cgController;

        public bool IsRunningConversation => ConversationManager.IsRunning;
        public bool IsInitialized => _isInitialized;
        private bool _isInitialized;

        // Events (IDialogueService)
        public event System.Action OnDialogueStarted;
        public event System.Action OnDialogueEnded;
        public event System.Action<string> OnSpeakerChanged;
        
        // Legacy Events (for compatibility)
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
            ConversationManager = new ConversationManager(_textArchitect);
            
            _cgController = new CanvasGroupController(this, mainCanvas);
            dialogueContainer.Initialize();    
            
            if (TryGetComponent(out _autoReader))
                _autoReader.Initialize(ConversationManager);
        }
        
        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            var character = CharacterManager.Instance.GetCharacter(speakerName);
            var characterConfigData = character != null ? character.Config : CharacterManager.Instance.GetCharacterConfig(speakerName);

            ApplySpeakerDataToDialogueContainer(characterConfigData);
        }

        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData configuration)
        {
            dialogueContainer.SetDialogueColor(configuration.dialogueColor);
            dialogueContainer.SetDialogueFont(configuration.dialogueFont);
            var fontSize = config.defaultDialogueFontSize * configuration.dialogueFontScale;
            dialogueContainer.SetDialogueFontSize(fontSize);
            
            dialogueContainer.nameContainer.SetNameColor(configuration.nameColor);
            dialogueContainer.nameContainer.SetNameFont(configuration.nameFont);
            fontSize = config.defaultNameFontSize * configuration.nameFontScale;
            dialogueContainer.nameContainer.SetNameFontSize(fontSize);
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator")
            {
                dialogueContainer.nameContainer.Show(speakerName);
                OnSpeakerChanged?.Invoke(speakerName);
                DialogueEvents.InvokeSpeakerChanged(speakerName);
                DialogueEvents.InvokeSpeakerNameShown(speakerName);
            }
            else
                HideSpeakerName();
        }

        public void HideSpeakerName()
        {
            dialogueContainer.nameContainer.Hide();
            DialogueEvents.InvokeSpeakerNameHidden();
        }

        public Coroutine Say(string speaker, string dialogue)
        {
            var conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> lines)
        {
            var conversation = new Conversation(lines);
            return Say(conversation);
        }

        public Coroutine Say(Conversation conversation)
        {
            OnDialogueStarted?.Invoke();
            DialogueEvents.InvokeDialogueStarted();
            DialogueEvents.InvokeConversationStarted(conversation);
            
            return ConversationManager.StartConversation(conversation);
        }

        public void OnUserPromptNext()
        {
            // Legacy event
            OnUserPromptNextEvent?.Invoke();
            
            // Note: OnUserPromptNext event is handled by InputSystem
            // DialogueSystem only handles the logic, not the event
            
            if(_autoReader != null && _autoReader.isOn)
                _autoReader.Disable();
        }

        public void OnSystemPromptNext()
        {
            // Legacy event
            OnUserPromptNextEvent?.Invoke();
            
            // Note: OnUserPromptNext event is handled by InputSystem
            // DialogueSystem only handles the logic, not the event
        }
        
        public Coroutine Show(float speed = 1f, bool immediate = false)
        {
            DialogueEvents.InvokeDialogueUIShown();
            return _cgController.Show(speed, immediate);
        }

        public Coroutine Hide(float speed = 1f, bool immediate = false)
        {
            DialogueEvents.InvokeDialogueUIHidden();
            return _cgController.Hide(speed, immediate);
        }
    }
}
