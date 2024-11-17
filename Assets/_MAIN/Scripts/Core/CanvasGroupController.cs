using System.Collections;
using _MAIN.Scripts.Core.Dialogue;
using UnityEngine;

namespace _MAIN.Scripts.Core
{
    public class CanvasGroupController
    {
        private const float DefaultFadeSpeed = 3f;

        private MonoBehaviour _owner;
        private CanvasGroup _rootCg;

        private Coroutine _coShowing;
        private Coroutine _coHiding;
        public bool IsShowing => _coShowing != null;
        public bool IsHiding => _coHiding != null;
        public bool IsFading => IsShowing || IsHiding;

        public bool IsVisible => _coShowing != null || _rootCg.alpha > 0;

        public float Alpha
        {
            get => _rootCg.alpha;
            set => _rootCg.alpha = value;
        }

        public CanvasGroupController(MonoBehaviour owner, CanvasGroup rootCg)
        {
            _owner = owner;
            _rootCg = rootCg;
        }

        public Coroutine Show(float speed = 1f, bool immediate = false)
        {
            if (IsShowing)
                return _coShowing;

            if (IsHiding)
            {
                DialogueSystem.Instance.StopCoroutine(_coHiding);
                _coHiding = null;
            }

            _coShowing = DialogueSystem.Instance.StartCoroutine(Fading(1, speed, immediate));

            return _coShowing;
        }

        public Coroutine Hide(float speed = 1f, bool immediate = false)
        {
            if (IsHiding)
                return _coHiding;

            if (IsShowing)
            {
                DialogueSystem.Instance.StopCoroutine(_coShowing);
                _coShowing = null;
            }

            _coHiding = DialogueSystem.Instance.StartCoroutine(Fading(0, speed, immediate));

            return _coHiding;
        }

        private IEnumerator Fading(float alpha, float speed, bool immediate)
        {
            CanvasGroup cg = _rootCg;

            if (immediate)
                cg.alpha = alpha;

            while (!Mathf.Approximately(cg.alpha, alpha))
            {
                cg.alpha = Mathf.MoveTowards(cg.alpha, alpha, Time.deltaTime * DefaultFadeSpeed * speed);
                yield return null;
            }

            _coShowing = null;
            _coHiding = null;
        }

        public void SetInteractableState(bool active)
        {
            _rootCg.interactable = active;
            _rootCg.blocksRaycasts = active;
        }
    }
}
