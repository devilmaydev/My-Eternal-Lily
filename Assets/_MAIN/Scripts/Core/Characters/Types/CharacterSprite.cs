using System.Collections;
using UnityEngine;

namespace _MAIN.Scripts.Core.Characters.Types
{
    public class CharacterSprite : Character
    {
        private CanvasGroup RootCg => Root.GetComponent<CanvasGroup>();

        public CharacterSprite(string name, CharacterConfigData config, GameObject prefab) : base(name, config, prefab)
        {
            RootCg.alpha = 0;
            
            Debug.Log($"Created Sprite Character: '{name}'");
        }

        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = RootCg;
            
            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }

            CoRevealing = null;
            CoHiding = null;
        }
    }
}