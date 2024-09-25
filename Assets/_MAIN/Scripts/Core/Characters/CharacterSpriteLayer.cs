using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace _MAIN.Scripts.Core.Characters
{
    public class CharacterSpriteLayer
    {
        private CharacterManager CharacterManager => CharacterManager.Instance;

        private const float DefaultTransitionSpeed = 3f;
        private float _transitionSpeedMultiplier = 1;

        public int Layer { get; private set; } = 0;
        public Image Renderer { get; private set; } = null;
        public CanvasGroup RendererCg => Renderer.GetComponent<CanvasGroup>();

        private List<CanvasGroup> _oldRenderers = new List<CanvasGroup>();

        private Coroutine _coTransitioningLayer = null;
        private Coroutine _coLevelingAlpha = null;
        private Coroutine _coChangingColor = null;
        public bool IsTransitioningLayer => _coTransitioningLayer != null;
        public bool IsLevelingAlpha => _coLevelingAlpha != null;
        public bool IsChangingColor => _coChangingColor != null;

        public CharacterSpriteLayer(Image defaultRenderer, int layer = 0)
        {
            Renderer = defaultRenderer;
            Layer = layer; 
        }

        public void SetSprite(Sprite sprite)
        {
            Renderer.sprite = sprite;
        }

        public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
        {
            if (sprite == Renderer.sprite)
                return null;

            if (IsTransitioningLayer)
                CharacterManager.StopCoroutine(_coTransitioningLayer);

            _coTransitioningLayer = CharacterManager.StartCoroutine(TransitioningSprite(sprite, speed));

            return _coTransitioningLayer;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier)
        {
            _transitionSpeedMultiplier = speedMultiplier;

            Image newRenderer = CreateRenderer(Renderer.transform.parent);
            newRenderer.sprite = sprite;

            yield return TryStartLevelingAlphas();

            _coTransitioningLayer = null;
        }

        private Image CreateRenderer(Transform parent)
        {
            Image newRenderer = Object.Instantiate(Renderer, parent);
            _oldRenderers.Add(RendererCg);

            newRenderer.name = Renderer.name;
            Renderer = newRenderer;
            Renderer.gameObject.SetActive(true);
            RendererCg.alpha = 0;

            return newRenderer;
        }

        private Coroutine TryStartLevelingAlphas()
        {
            if (IsLevelingAlpha)
                return _coLevelingAlpha;

            _coLevelingAlpha = CharacterManager.StartCoroutine(RunAlphaLeveling());

            return _coLevelingAlpha;
        }

        private IEnumerator RunAlphaLeveling()
        {
            while (RendererCg.alpha < 1 || _oldRenderers.Any(oldCG => oldCG.alpha > 0))
            {
                float speed = DefaultTransitionSpeed * _transitionSpeedMultiplier * Time.deltaTime;

                RendererCg.alpha = Mathf.MoveTowards(RendererCg.alpha, 1, speed);

                for (int i = _oldRenderers.Count - 1; i >= 0; i--)
                {
                    CanvasGroup oldCG = _oldRenderers[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);

                    if (oldCG.alpha <= 0)
                    {
                        _oldRenderers.RemoveAt(i);
                        Object.Destroy(oldCG.gameObject);
                    }
                }

                yield return null;
            }

            _coLevelingAlpha = null;
        }

        public void SetColor(Color color)
        {
            Renderer.color = color;

            foreach (CanvasGroup oldCG in _oldRenderers)
            {
                oldCG.GetComponent<Image>().color = color;
            }
        }

        public Coroutine TransitionColor(Color color, float speed)
        {
            if (IsChangingColor)
                CharacterManager.StopCoroutine(_coChangingColor);

            _coChangingColor = CharacterManager.StartCoroutine(ChangingColor(color, speed));

            return _coChangingColor;
        }

        public void StopChangingColor()
        {
            if (!IsChangingColor)
                return;

            CharacterManager.StopCoroutine(_coChangingColor);

            _coChangingColor = null;
        }

        private IEnumerator ChangingColor(Color color, float speedMultiplier)
        {
            Color oldColor = Renderer.color;
            List<Image> oldImages = new List<Image>();

            foreach (var oldCG in _oldRenderers)
            {
                oldImages.Add(oldCG.GetComponent<Image>());
            }

            float colorPercent = 0;
            while (colorPercent < 1)
            {
                colorPercent += DefaultTransitionSpeed * speedMultiplier * Time.deltaTime;

                Renderer.color = Color.Lerp(oldColor, color, colorPercent);

                foreach (Image oldImage in oldImages)
                {
                    oldImage.color = Renderer.color;
                }

                yield return null;
            }

            _coChangingColor = null;
        }
    }
}
