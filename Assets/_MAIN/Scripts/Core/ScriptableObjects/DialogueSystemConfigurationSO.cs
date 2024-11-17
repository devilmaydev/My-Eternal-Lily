using _MAIN.Scripts.Core.Characters;
using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public const float DefaultFontsizeDialogue = 18;
        public const float DefaultFontsizeName = 22;
        
        public CharacterConfigSO characterConfigurationAsset;

        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;
        
        public float dialogueFontScale = 1f;
        public float defaultNameFontSize = DefaultFontsizeName;
        public float defaultDialogueFontSize = DefaultFontsizeDialogue;
    }
}
