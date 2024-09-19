using UnityEngine;

namespace _MAIN.Scripts.Core.Characters.Types
{
    public class CharacterSprite : Character
    {
        public CharacterSprite(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Created Sprite Character: '{name}'");
        }
    }
}