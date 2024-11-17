using System.Collections.Generic;
using System.Linq;
using _MAIN.Scripts.Core.Characters.Types;
using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.ScriptableObjects;
using _MAIN.Scripts.Enums;
using UnityEngine;

namespace _MAIN.Scripts.Core.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        private Dictionary<string, Character> _characters = new();
        public Character[] AllCharacters => _characters.Values.ToArray();


        private CharacterConfigSO Config => DialogueSystem.Instance.Config.characterConfigurationAsset;
        
        private const string CharacterCastingID = " as ";
        private const string CharacterNameID = "<charname>";
        public string CharacterRootPathFormat => $"Characters/{CharacterNameID}";
        public string CharacterPrefabNameFormat => $"Character - [{CharacterNameID}]";
        public string CharacterPrefabPathFormat => $"{CharacterRootPathFormat}/{CharacterPrefabNameFormat}";

        [SerializeField] private RectTransform characterPanel = null;
        public RectTransform CharacterPanel => characterPanel;
        private void Awake() => Instance = this;
        
        public CharacterConfigData GetCharacterConfig(string characterName, bool getOriginal = false)
        {
            if (!getOriginal)
            {
                var character = GetCharacter(characterName);
                
                if (character != null)
                    return character.Config;
            }
            
            return Config.GetConfig(characterName);
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if (_characters.ContainsKey(characterName.ToLower()))
                return _characters[characterName.ToLower()];
            
            return createIfDoesNotExist ? CreateCharacter(characterName) : null;
        }
        
        public bool HasCharacter(string characterName) => _characters.ContainsKey(characterName.ToLower());

        public Character CreateCharacter(string characterName, bool revealAfterCreated = false)
        {
            if (_characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the character.");
                return null;
            }

            var info = GetCharacterInfo(characterName);

            var character = CreateCharacterFromInfo(info);

            _characters.Add(info.Name.ToLower(), character);

            if (revealAfterCreated)
                character.Show();

            return character;
        }

        private CharacterInfo GetCharacterInfo(string characterName)
        {
            var result = new CharacterInfo();

            var nameData = characterName.Split(CharacterCastingID, System.StringSplitOptions.RemoveEmptyEntries);
            result.Name = nameData[0];
            result.CastingName = nameData.Length > 1 ? nameData[1] : result.Name;

            result.Config = Config.GetConfig(result.CastingName);

            result.Prefab = GetPrefabForCharacter(result.CastingName);
            
            result.RootCharacterFolder = FormatCharacterPath(CharacterRootPathFormat, result.CastingName);

            return result;
        }
        
        private GameObject GetPrefabForCharacter(string characterName)
        {
            var prefabPath = FormatCharacterPath(CharacterPrefabPathFormat, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        private Character CreateCharacterFromInfo(CharacterInfo info)
        {
            return info.Config.characterType switch
            {
                ECharacterType.Text => new CharacterText(info.Name, info.Config),
                ECharacterType.Sprite or ECharacterType.SpriteSheet => new CharacterSprite(info.Name, info.Config, info.Prefab, info.RootCharacterFolder),
                ECharacterType.Live2D => new CharacterLive2D(info.Name, info.Config, info.Prefab, info.RootCharacterFolder),
                ECharacterType.Model3D => new CharacterModel3D(info.Name, info.Config, info.Prefab, info.RootCharacterFolder),
                _ => null
            };
        }

        public void SortCharacters()
        {
            var activeCharacters = _characters.Values.Where(c => c.Root.gameObject.activeInHierarchy && c.IsVisible).ToList();
            var inactiveCharacters = _characters.Values.Except(activeCharacters).ToList();
            
            activeCharacters.Sort((a, b) 
                => a.Priority.CompareTo(b.Priority)
            );

            activeCharacters.Concat(inactiveCharacters);

            SortCharacters(activeCharacters);
        }

        public void SortCharacters(string[] charactersNames)
        {
            var sortedCharacters = charactersNames
                .Select(name => GetCharacter(name))
                .Where(character => character != null)
                .ToList();

            var remainingCharacters = _characters.Values
                .Except(sortedCharacters)
                .OrderBy(character => character.Priority)
                .ToList();

            sortedCharacters.Reverse();
            
            var startingPriority = remainingCharacters.Count > 0 ? remainingCharacters.Max(c => c.Priority) : 0;
            for (int i = 0; i < sortedCharacters.Count; i++)
            {
                var character = sortedCharacters[i];
                character.SetPriority(startingPriority + i + 1, autoSortCharactersOnUI:false);
            }
            
            var allCharacters = remainingCharacters.Concat(sortedCharacters).ToList();
            
            SortCharacters(allCharacters);
        }

        private void SortCharacters(List<Character> charactersSorted)
        {
            var i = 0;
            foreach (var character in charactersSorted)
                character.Root.SetSiblingIndex(i++);
        }
        
        public string FormatCharacterPath(string path, string characterName) => path.Replace(CharacterNameID, characterName);

        private class CharacterInfo
        {
            public string Name = "";
            public string CastingName;
            public string RootCharacterFolder;
            public CharacterConfigData Config;
            public GameObject Prefab;
        }
    }
}