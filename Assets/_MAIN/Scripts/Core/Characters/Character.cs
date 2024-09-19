using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue;
using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.Characters
{
    public abstract class Character
    {
        public string Name;
        public string DisplayName;
        public RectTransform Root = null;
        public CharacterConfigData Config;

        public DialogueSystem DialogueSystem => DialogueSystem.Instance;

        public Character(string name, CharacterConfigData config)
        {
            Name = name;
            DisplayName = name;
            Config = config;
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue)
        {
            DialogueSystem.ShowSpeakerName(DisplayName);
            //TODO: FIX THIS SHIT SOON DONT FORGET
            UpdateTextCustomizationsOnScreen();
            return DialogueSystem.Say(dialogue);
        }

        public void SetNameFont(TMP_FontAsset font) => Config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => Config.dialogueFont = font;
        public void SetNameColor(Color color) => Config.nameColor = color;
        public void SetDialogueColor(Color color) => Config.dialogueColor = color;
        public void ResetConfigurationData() => Config = CharacterManager.Instance.GetCharacterConfig(Name);
        public void UpdateTextCustomizationsOnScreen() => DialogueSystem.ApplySpeakerDataToDialogueContainer(Config);
    
    }
}
