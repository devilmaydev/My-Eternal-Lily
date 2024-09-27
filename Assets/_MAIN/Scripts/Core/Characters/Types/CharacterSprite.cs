using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _MAIN.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _MAIN.Scripts.Core.Characters.Types
{
    public class CharacterSprite : Character
    {
        private const string SpriteRendererParentName = "Renderer";
        private const string SpritesheetDefaultSheetname = "Default";
        private const char SpritesheetTEXSpriteDelimitter = '-';
        private CanvasGroup RootCg => Root.GetComponent<CanvasGroup>();

        public List<CharacterSpriteLayer> Layers = new ();

        private string _artAssetsDirectory = "";

        public override bool IsVisible
        {
            get => IsRevealing || RootCg.alpha == 1;
            set => RootCg.alpha = value ? 1 : 0;
        }


        public CharacterSprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            RootCg.alpha = 0;
            
            RootCg.alpha = EnableOnStart ? 1 : 0;
            _artAssetsDirectory = rootAssetsFolder + "/Images";

            GetLayers();
            
            Debug.Log($"Created Sprite Character: '{name}'");
        }
        
        private void GetLayers()
        {
            Transform rendererRoot = Animator.transform.Find(SpriteRendererParentName);

            if (rendererRoot == null)
                return;

            for (int i = 0; i < rendererRoot.transform.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);

                Image rendererImage = child.GetComponent<Image>();

                if (rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    Layers.Add(layer);
                    child.name = $"Layer: {i}";
                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            Layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if (Config.characterType == ECharacterType.SpriteSheet)
            {
                string[] data = spriteName.Split(SpritesheetTEXSpriteDelimitter);
                Sprite[] spriteArray = new Sprite[0]; 

                if (data.Length == 2)
                {
                    string textureName = data[0];
                    spriteName = data[1];
                    spriteArray = Resources.LoadAll<Sprite>($"{_artAssetsDirectory}/{textureName}");
                }
                else
                {
                    spriteArray = Resources.LoadAll<Sprite>($"{_artAssetsDirectory}/{SpritesheetDefaultSheetname}");
                }

                if (spriteArray.Length == 0)
                    Debug.LogWarning($"Character '{Name}' does not have a default art asset called '{SpritesheetDefaultSheetname}'");

                return Array.Find(spriteArray, sprite => sprite.name == spriteName);
            }
            else
                return Resources.Load<Sprite>($"{_artAssetsDirectory}/{spriteName}");
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer spriteLayer = Layers[layer];

            return spriteLayer.TransitionSprite(sprite, speed);
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
        
        public override void SetColor(Color color)
        {
            base.SetColor(color);

            color = DisplayColor;

            foreach (CharacterSpriteLayer layer in Layers)
            {
                layer.StopChangingColor();
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            foreach (CharacterSpriteLayer layer in Layers)
                layer.TransitionColor(color, speed);

            yield return null;

            while (Layers.Any(l => l.IsChangingColor))
                yield return null;

            CoChangingColor = null;
        }

        public override IEnumerator Highlighting(bool highlight, float speedMultiplier)
        {
            Color targetColor = DisplayColor;

            foreach (CharacterSpriteLayer layer in Layers)
                layer.TransitionColor(targetColor, speedMultiplier);

            yield return null;

            while (Layers.Any(l => l.IsChangingColor))
                yield return null;

            CoHighlighting = null;
        }
    }
}