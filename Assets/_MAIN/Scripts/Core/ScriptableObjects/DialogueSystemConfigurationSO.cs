using _MAIN.Scripts.Core.Characters;
using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigurationAsset;

        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;
    }
}
