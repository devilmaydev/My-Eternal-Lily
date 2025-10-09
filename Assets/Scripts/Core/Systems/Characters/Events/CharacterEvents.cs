using UnityEngine;
using Core.Characters;

namespace Core.Systems.Characters.Events
{
    /// <summary>
    /// Eventos centralizados do sistema de personagens
    /// Permite comunicação desacoplada entre sistemas
    /// </summary>
    public static class CharacterEvents
    {
        // Character Lifecycle Events
        public static event System.Action<Character> OnCharacterCreated;
        public static event System.Action<Character> OnCharacterDestroyed;
        public static event System.Action<Character> OnCharacterShown;
        public static event System.Action<Character> OnCharacterHidden;
        
        // Character State Events
        public static event System.Action<Character, int> OnCharacterPriorityChanged;
        public static event System.Action<Character, Color> OnCharacterColorChanged;
        public static event System.Action<Character, bool> OnCharacterHighlighted;
        
        // Character Movement Events
        public static event System.Action<Character, Vector2> OnCharacterMoved;
        public static event System.Action<Character, Vector2> OnCharacterMoveStarted;
        public static event System.Action<Character, Vector2> OnCharacterMoveCompleted;
        
        // Character Animation Events
        public static event System.Action<Character> OnCharacterAnimationStarted;
        public static event System.Action<Character> OnCharacterAnimationCompleted;
        public static event System.Action<Character, string> OnCharacterAnimationChanged;
        
        // Character Sorting Events
        public static event System.Action OnCharactersSorted;
        public static event System.Action<string[]> OnCharactersSortedByNames;
        
        // Character Configuration Events
        public static event System.Action<Character, CharacterConfigData> OnCharacterConfigChanged;
        public static event System.Action<string> OnCharacterConfigLoaded;
        
        // Event Invokers
        public static void InvokeCharacterCreated(Character character) => OnCharacterCreated?.Invoke(character);
        public static void InvokeCharacterDestroyed(Character character) => OnCharacterDestroyed?.Invoke(character);
        public static void InvokeCharacterShown(Character character) => OnCharacterShown?.Invoke(character);
        public static void InvokeCharacterHidden(Character character) => OnCharacterHidden?.Invoke(character);
        
        public static void InvokeCharacterPriorityChanged(Character character, int priority) => OnCharacterPriorityChanged?.Invoke(character, priority);
        public static void InvokeCharacterColorChanged(Character character, Color color) => OnCharacterColorChanged?.Invoke(character, color);
        public static void InvokeCharacterHighlighted(Character character, bool highlighted) => OnCharacterHighlighted?.Invoke(character, highlighted);
        
        public static void InvokeCharacterMoved(Character character, Vector2 position) => OnCharacterMoved?.Invoke(character, position);
        public static void InvokeCharacterMoveStarted(Character character, Vector2 targetPosition) => OnCharacterMoveStarted?.Invoke(character, targetPosition);
        public static void InvokeCharacterMoveCompleted(Character character, Vector2 finalPosition) => OnCharacterMoveCompleted?.Invoke(character, finalPosition);
        
        public static void InvokeCharacterAnimationStarted(Character character) => OnCharacterAnimationStarted?.Invoke(character);
        public static void InvokeCharacterAnimationCompleted(Character character) => OnCharacterAnimationCompleted?.Invoke(character);
        public static void InvokeCharacterAnimationChanged(Character character, string animationName) => OnCharacterAnimationChanged?.Invoke(character, animationName);
        
        public static void InvokeCharactersSorted() => OnCharactersSorted?.Invoke();
        public static void InvokeCharactersSortedByNames(string[] characterNames) => OnCharactersSortedByNames?.Invoke(characterNames);
        
        public static void InvokeCharacterConfigChanged(Character character, CharacterConfigData config) => OnCharacterConfigChanged?.Invoke(character, config);
        public static void InvokeCharacterConfigLoaded(string characterName) => OnCharacterConfigLoaded?.Invoke(characterName);
    }
}
