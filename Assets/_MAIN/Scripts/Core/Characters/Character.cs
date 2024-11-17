using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue;
using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.Characters
{
    public abstract class Character
    {
        public const bool EnableOnStart = false;
        private const float UnhighlightedDarkenStrength = 0.65f;
        
        public string Name;
        public string DisplayName;
        public RectTransform Root = null;
        public CharacterConfigData Config;
        public Animator Animator;
        
        public Color Color { get; protected set; } = Color.white;
        protected Color DisplayColor => Highlighted ? HighlightedColor : UnhighlightedColor;
        protected Color HighlightedColor => Color;
        protected Color UnhighlightedColor => new(Color.r * UnhighlightedDarkenStrength, Color.g * UnhighlightedDarkenStrength, Color.b * UnhighlightedDarkenStrength, Color.a);
        public bool Highlighted { get; protected set; } = true;
        public int Priority { get; protected set; }

        protected Coroutine CoRevealing;
        protected Coroutine CoHiding;
        protected Coroutine CoMoving;
        protected Coroutine CoChangingColor;
        protected Coroutine CoHighlighting;

        public bool IsRevealing => CoRevealing != null;
        public bool IsHiding => CoHiding != null;
        public bool IsMoving => CoMoving != null;
        public bool IsChangingColor => CoChangingColor != null;
        public bool IsHighlighting => Highlighted && CoHighlighting != null;
        public bool IsUnHighlighting => !Highlighted && CoHighlighting != null;
        
        public virtual bool IsVisible { get; set; }
        
        public DialogueSystem DialogueSystem => DialogueSystem.Instance;
        protected CharacterManager CharacterManager => CharacterManager.Instance;
        

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            Name = name;
            DisplayName = name;
            Config = config;
            
            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, CharacterManager.CharacterPanel);
                ob.name = CharacterManager.FormatCharacterPath(CharacterManager.CharacterPrefabNameFormat, name);
                ob.SetActive(true);
                Root = ob.GetComponent<RectTransform>();
                Animator = Root.GetComponentInChildren<Animator>();
            }
        }
        
        public void SetNameFont(TMP_FontAsset font) => Config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => Config.dialogueFont = font;
        public void SetNameColor(Color color) => Config.nameColor = color;
        public void SetDialogueColor(Color color) => Config.dialogueColor = color;
        public void ResetConfigurationData() => Config = CharacterManager.Instance.GetCharacterConfig(Name, true);
        public void UpdateTextCustomizationsOnScreen() => DialogueSystem.ApplySpeakerDataToDialogueContainer(Config);

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue)
        {
            DialogueSystem.ShowSpeakerName(DisplayName);
            UpdateTextCustomizationsOnScreen();
            return DialogueSystem.Say(dialogue);
        }
        
        public virtual Coroutine Show(float speedMultiplier = 1f)
        {
            if (IsRevealing)
                return CoRevealing;

            if (IsHiding)
                CharacterManager.StopCoroutine(CoHiding);

            CoRevealing = CharacterManager.StartCoroutine(ShowingOrHiding(true, speedMultiplier));

            return CoRevealing;
        }

        public virtual Coroutine Hide(float speedMultiplier = 1f)
        {
            if (IsHiding)
                return CoHiding;

            if (IsRevealing)
                CharacterManager.StopCoroutine(CoRevealing);

            CoHiding = CharacterManager.StartCoroutine(ShowingOrHiding(false, speedMultiplier));

            return CoHiding;
        }

        public virtual IEnumerator ShowingOrHiding(bool show, float speedMultiplier)
        {
            Debug.Log("Show/Hide cannot be called from a base character type.");
            yield return null;
        }

        public virtual void SetPosition(Vector2 position)
        {
            if (Root == null)
                return;

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);

            Root.anchorMin = minAnchorTarget;
            Root.anchorMax = maxAnchorTarget;
        }
        
        public virtual Coroutine MoveToPosition(Vector2 positon, float speed = 2f, bool smooth = false)
        {
            if (Root == null)
                return null;

            if (IsMoving)
                CharacterManager.StopCoroutine(CoMoving);

            CoMoving = CharacterManager.StartCoroutine(MovingToPosition(positon, speed, smooth));

            return CoMoving;
        }

        private IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = Root.anchorMax - Root.anchorMin;

            while (Root.anchorMin != minAnchorTarget || Root.anchorMax != maxAnchorTarget)
            {
                Root.anchorMin = smooth ?
                    Vector2.Lerp(Root.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                    : Vector2.MoveTowards(Root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                Root.anchorMax = Root.anchorMin + padding;

                if (smooth && Vector2.Distance(Root.anchorMin, minAnchorTarget) <= 0.001f)
                {
                    Root.anchorMin = minAnchorTarget;
                    Root.anchorMax = maxAnchorTarget;
                    break;
                }

                yield return null;
            }

            Debug.Log("Done moving");
            CoMoving = null;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position)
        {
            Vector2 padding = Root.anchorMax - Root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
        }
        
        public virtual void SetColor(Color color) => Color = color;

        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            Color = color;
        
            if (IsChangingColor)
                CharacterManager.StopCoroutine(CoChangingColor);
        
            CoChangingColor = CharacterManager.StartCoroutine(ChangingColor(DisplayColor, speed));
        
            return CoChangingColor;
        }

        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            yield return null;
        }

        public Coroutine Highlight(float speed = 1f, bool immediate = false)
        {
            if (IsHighlighting)
                return CoHighlighting;
        
            if (IsUnHighlighting)
                CharacterManager.StopCoroutine(CoHighlighting);
        
            Highlighted = true;
            CoHighlighting = CharacterManager.StartCoroutine(Highlighting(speed, immediate));
        
            return CoHighlighting;
        }
        
        public Coroutine UnHighlight(float speed = 1f, bool immediate = false)
        {
            if (IsUnHighlighting)
                return CoHighlighting;
        
            if (IsHighlighting)
                CharacterManager.StopCoroutine(CoHighlighting);
        
            Highlighted = false;
            CoHighlighting = CharacterManager.StartCoroutine(Highlighting(speed, immediate));
        
            return CoHighlighting;
        }

        public virtual IEnumerator Highlighting(float speedMultiplier, bool immediate = false)
        {
            Debug.Log("Highlighting is not available on this character type!");
            yield return null;
        }

        public void SetPriority(int priority, bool autoSortCharactersOnUI = true)
        {
            Priority = priority;
            if (autoSortCharactersOnUI)
                CharacterManager.SortCharacters();        
        }

        public virtual void OnSort(int sortingIndex) { }

        public virtual void OnReceiveCastingExpression(int layer, string expression) { }
        
    }
}
