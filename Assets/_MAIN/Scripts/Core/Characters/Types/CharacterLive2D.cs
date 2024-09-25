using UnityEngine;

namespace _MAIN.Scripts.Core.Characters.Types
{
    public class CharacterLive2D : Character
    {
        public CharacterLive2D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            Debug.Log($"Created Live2D Character: '{name}'");
        }
    }
}