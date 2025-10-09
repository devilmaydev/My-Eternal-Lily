using System.Collections.Generic;
using UnityEngine;
using Core.Characters;
using _MAIN.Scripts.Core.ScriptableObjects;

namespace Core.Systems.Characters.Interfaces
{
    /// <summary>
    /// Interface para o sistema de personagens
    /// Define contratos para gerenciamento de personagens
    /// </summary>
    public interface ICharacterService
    {
        // Events
        event System.Action<Character> OnCharacterCreated;
        event System.Action<Character> OnCharacterDestroyed;
        event System.Action<Character> OnCharacterShown;
        event System.Action<Character> OnCharacterHidden;
        event System.Action<Character, int> OnCharacterPriorityChanged;
        
        // Character State
        Character[] AllCharacters { get; }
        int CharacterCount { get; }
        
        // Character Management
        Character GetCharacter(string characterName, bool createIfDoesNotExist = false);
        Character CreateCharacter(string characterName, bool revealAfterCreated = false);
        bool HasCharacter(string characterName);
        void DestroyCharacter(string characterName);
        void DestroyAllCharacters();
        
        // Character Configuration
        CharacterConfigData GetCharacterConfig(string characterName, bool getOriginal = false);
        
        // Character Sorting
        void SortCharacters();
        void SortCharacters(string[] charactersNames);
        
        // Character Panel
        RectTransform CharacterPanel { get; }
        
        // Character Paths
        string CharacterRootPathFormat { get; }
        string CharacterPrefabNameFormat { get; }
        string CharacterPrefabPathFormat { get; }
        string FormatCharacterPath(string path, string characterName);
    }
}
