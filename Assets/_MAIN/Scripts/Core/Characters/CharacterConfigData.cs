using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Enums;
using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.Characters
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public ECharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;

        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;
            result.nameFont = nameFont;
            result.dialogueFont = dialogueFont;

            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            return result;
        }

        private static Color DefaultColor => DialogueSystem.Instance.Config.defaultTextColor;
        private static TMP_FontAsset DefaultFont => DialogueSystem.Instance.Config.defaultFont;
        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = "";
                result.alias = "";
                result.characterType = ECharacterType.Text;
                
                result.nameFont = DefaultFont;
                result.dialogueFont = DefaultFont;
                result.nameColor = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, DefaultColor.a);
                result.dialogueColor = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, DefaultColor.a);

                return result;
            }
        }

    }
}