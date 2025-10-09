using System.Collections;
using System.Collections.Generic;
using Core.Characters;
using UnityEngine;
using Core.Dialogue.DataContainers;
using Core.Dialogue.Managers;

namespace Core.Systems.Dialogue.Interfaces
{
    /// <summary>
    /// Interface para o sistema de diálogo
    /// Define contratos para gerenciamento de diálogos
    /// </summary>
    public interface IDialogueService
    {
        // Events
        event System.Action OnDialogueStarted;
        event System.Action OnDialogueEnded;
        event System.Action<string> OnSpeakerChanged;
        
        // Dialogue State
        bool IsRunningConversation { get; }
        bool IsInitialized { get; }
        
        // Dialogue Methods
        Coroutine Say(string speaker, string dialogue);
        Coroutine Say(List<string> lines);
        Coroutine Say(Conversation conversation);
        
        // UI Control
        Coroutine Show(float speed = 1f, bool immediate = false);
        Coroutine Hide(float speed = 1f, bool immediate = false);
        
        // Speaker Management
        void ApplySpeakerDataToDialogueContainer(string speakerName);
        void ApplySpeakerDataToDialogueContainer(CharacterConfigData configuration);
        void ShowSpeakerName(string speakerName = "");
        void HideSpeakerName();
        
        // Input Handling
        void OnUserPromptNext();
        void OnSystemPromptNext();
    }
}
