using UnityEngine;

namespace Core.Systems.Characters.Settings
{
    /// <summary>
    /// Configurações do sistema de personagens
    /// Centraliza todas as configurações de personagens
    /// </summary>
    [CreateAssetMenu(fileName = "VnCharacterSettings", menuName = "Visual Novel/Character Settings")]
    public class VnCharacterSettings : ScriptableObject
    {
        [Header("Character Creation Settings")]
        public bool enableCharacterCaching = true;
        public bool enableCharacterPooling = false;
        public int maxCharacters = 20;
        public bool autoCreateOnDemand = true;
        public bool enableCharacterValidation = true;
        
        [Header("Character Paths")]
        public string characterRootPathFormat = "Characters/{0}";
        public string characterPrefabNameFormat = "Character - [{0}]";
        public string characterPrefabPathFormat = "Characters/{0}/Character - [{0}]";
        public string characterCastingID = " as ";
        public string characterNameID = "<charname>";
        
        [Header("Character Sorting")]
        public bool enableAutoSorting = true;
        public bool enablePrioritySorting = true;
        public bool enableZOrderSorting = true;
        public int defaultPriority = 0;
        
        [Header("Character Animation")]
        public bool enableCharacterAnimations = true;
        public float defaultAnimationSpeed = 1f;
        public bool enableAnimationBlending = true;
        public bool enableAnimationEvents = true;
        
        [Header("Character Movement")]
        public bool enableCharacterMovement = true;
        public float defaultMoveSpeed = 1f;
        public bool enableSmoothMovement = true;
        public bool enableMovementEvents = true;
        
        [Header("Character Highlighting")]
        public bool enableCharacterHighlighting = true;
        public float unhighlightedDarkenStrength = 0.65f;
        public bool enableHighlightingAnimations = true;
        public float highlightingAnimationSpeed = 1f;
        
        [Header("Character Colors")]
        public bool enableCharacterColors = true;
        public Color defaultCharacterColor = Color.white;
        public bool enableColorAnimations = true;
        public float colorAnimationSpeed = 1f;
        
        [Header("Performance Settings")]
        public bool enableCharacterCulling = true;
        public bool enableCharacterLOD = false;
        public int characterPoolSize = 10;
        public bool enableCharacterPreloading = false;
        
        [Header("Debug Settings")]
        public bool enableDebugMode = false;
        public bool enableCharacterTrace = false;
        public bool enableAnimationTrace = false;
        public bool enableMovementTrace = false;
        
        /// <summary>
        /// Valida as configurações
        /// </summary>
        public bool ValidateSettings()
        {
            if (maxCharacters <= 0)
            {
                Debug.LogError("VnCharacterSettings: Max characters must be greater than 0!");
                return false;
            }
            
            if (defaultAnimationSpeed <= 0)
            {
                Debug.LogError("VnCharacterSettings: Default animation speed must be greater than 0!");
                return false;
            }
            
            if (defaultMoveSpeed <= 0)
            {
                Debug.LogError("VnCharacterSettings: Default move speed must be greater than 0!");
                return false;
            }
            
            if (unhighlightedDarkenStrength < 0 || unhighlightedDarkenStrength > 1)
            {
                Debug.LogError("VnCharacterSettings: Unhighlighted darken strength must be between 0 and 1!");
                return false;
            }
            
            if (highlightingAnimationSpeed <= 0)
            {
                Debug.LogError("VnCharacterSettings: Highlighting animation speed must be greater than 0!");
                return false;
            }
            
            if (colorAnimationSpeed <= 0)
            {
                Debug.LogError("VnCharacterSettings: Color animation speed must be greater than 0!");
                return false;
            }
            
            if (characterPoolSize <= 0)
            {
                Debug.LogError("VnCharacterSettings: Character pool size must be greater than 0!");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Aplica as configurações iniciais
        /// </summary>
        public void ApplyInitialSettings()
        {
            // Configurações iniciais serão aplicadas pelo CharacterManager
            Debug.Log("VnCharacterSettings: Initial settings applied");
        }
    }
}
