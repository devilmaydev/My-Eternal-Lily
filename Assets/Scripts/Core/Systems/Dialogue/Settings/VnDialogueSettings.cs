using UnityEngine;

namespace Core.Systems.Dialogue.Settings
{
    /// <summary>
    /// Configurações do sistema de diálogo
    /// Centraliza todas as configurações de diálogo
    /// </summary>
    [CreateAssetMenu(fileName = "VnDialogueSettings", menuName = "Visual Novel/Dialogue Settings")]
    public class VnDialogueSettings : ScriptableObject
    {
        [Header("Text Settings")]
        public float defaultDialogueFontSize = 24f;
        public float defaultNameFontSize = 20f;
        public Font defaultDialogueFont;
        public Font defaultNameFont;
        public Color defaultDialogueColor = Color.white;
        public Color defaultNameColor = Color.yellow;
        
        [Header("Animation Settings")]
        public float textSpeed = 1f;
        public float fadeSpeed = 1f;
        public bool enableTextAnimation = true;
        public bool enableFadeAnimation = true;
        
        [Header("Auto Reader Settings")]
        public float autoReadDelay = 2f;
        public bool enableAutoReader = false;
        public bool autoReaderEnabledByDefault = false;
        
        [Header("Input Settings")]
        public bool enableSkipOnClick = true;
        public bool enableSkipOnSpace = true;
        public bool enableSkipOnEnter = true;
        
        [Header("UI Settings")]
        public bool showSpeakerName = true;
        public bool hideSpeakerNameForNarrator = true;
        public bool enableDialogueBox = true;
        public bool enableContinuePrompt = true;
        
        [Header("Performance Settings")]
        public int maxConversationQueueSize = 10;
        public bool enableConversationCaching = true;
        public bool enableTextCaching = true;
        
        /// <summary>
        /// Valida as configurações
        /// </summary>
        public bool ValidateSettings()
        {
            if (defaultDialogueFontSize <= 0)
            {
                Debug.LogError("VnDialogueSettings: Default dialogue font size must be greater than 0!");
                return false;
            }
            
            if (defaultNameFontSize <= 0)
            {
                Debug.LogError("VnDialogueSettings: Default name font size must be greater than 0!");
                return false;
            }
            
            if (textSpeed <= 0)
            {
                Debug.LogError("VnDialogueSettings: Text speed must be greater than 0!");
                return false;
            }
            
            if (fadeSpeed <= 0)
            {
                Debug.LogError("VnDialogueSettings: Fade speed must be greater than 0!");
                return false;
            }
            
            if (autoReadDelay <= 0)
            {
                Debug.LogError("VnDialogueSettings: Auto read delay must be greater than 0!");
                return false;
            }
            
            if (maxConversationQueueSize <= 0)
            {
                Debug.LogError("VnDialogueSettings: Max conversation queue size must be greater than 0!");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Aplica as configurações iniciais
        /// </summary>
        public void ApplyInitialSettings()
        {
            // Configurações iniciais serão aplicadas pelo DialogueSystem
            Debug.Log("VnDialogueSettings: Initial settings applied");
        }
    }
}
