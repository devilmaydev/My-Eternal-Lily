using Core.Dialogue.Managers;
using UnityEngine;

namespace Core.Systems.Dialogue.Events
{
    /// <summary>
    /// Eventos centralizados do sistema de diálogo
    /// Permite comunicação desacoplada entre sistemas
    /// </summary>
    public static class DialogueEvents
    {
        // Dialogue Events
        public static event System.Action OnDialogueStarted;
        public static event System.Action OnDialogueEnded;
        public static event System.Action<string> OnSpeakerChanged;
        public static event System.Action<Conversation> OnConversationStarted;
        public static event System.Action<Conversation> OnConversationEnded;
        
        // Text Events
        public static event System.Action<string> OnTextStarted;
        public static event System.Action<string> OnTextCompleted;
        public static event System.Action OnTextSkipped;
        
        // UI Events
        public static event System.Action OnDialogueUIShown;
        public static event System.Action OnDialogueUIHidden;
        public static event System.Action<string> OnSpeakerNameShown;
        public static event System.Action OnSpeakerNameHidden;
        
        // Event Invokers
        public static void InvokeDialogueStarted() => OnDialogueStarted?.Invoke();
        public static void InvokeDialogueEnded() => OnDialogueEnded?.Invoke();
        public static void InvokeSpeakerChanged(string speakerName) => OnSpeakerChanged?.Invoke(speakerName);
        public static void InvokeConversationStarted(Conversation conversation) => OnConversationStarted?.Invoke(conversation);
        public static void InvokeConversationEnded(Conversation conversation) => OnConversationEnded?.Invoke(conversation);
        
        public static void InvokeTextStarted(string text) => OnTextStarted?.Invoke(text);
        public static void InvokeTextCompleted(string text) => OnTextCompleted?.Invoke(text);
        public static void InvokeTextSkipped() => OnTextSkipped?.Invoke();
        
        public static void InvokeDialogueUIShown() => OnDialogueUIShown?.Invoke();
        public static void InvokeDialogueUIHidden() => OnDialogueUIHidden?.Invoke();
        public static void InvokeSpeakerNameShown(string speakerName) => OnSpeakerNameShown?.Invoke(speakerName);
        public static void InvokeSpeakerNameHidden() => OnSpeakerNameHidden?.Invoke();
    }
}
