using UnityEngine;

namespace _MAIN.Scripts.Core.Characters.Types
{
    public class CharacterText : Character
    {
        public CharacterText(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Created Text Character: '{name}'");
        }
    }
}