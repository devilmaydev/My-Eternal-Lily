using System.Collections;
using _MAIN.Scripts.Enums;
using UnityEngine;
using TMPro;

namespace _MAIN.Scripts.Core
{
    public class TextArchitect
    {
        private TextMeshProUGUI _tmProUI;
        private TextMeshPro _tmProWorld;
        
        public TMP_Text TmPro => _tmProUI != null ? _tmProUI : _tmProWorld;

        public string CurrentText => TmPro.text;
        public string TargetText { get; private set; } = "";
        public string PreText { get; private set; } = "";
        private int _preTextLength = 0;
        
        public string FullTargetText => PreText + TargetText;

        public EBuildMethod BuildMethodChosen = EBuildMethod.Typewriter;
        
        public Color TextColor
        {
            get => TmPro.color;
            set => TmPro.color = value;
        }

        private const float BaseSpeed = 1;
        private float _speedMultiplier = 1;

        public float Speed
        {
            get => BaseSpeed * _speedMultiplier;
            set => _speedMultiplier = value;
        }

        public int CharacterPorCycle =>
            Speed <= 2f ? _characterMultiplier :
            Speed <= 2.5f ? _characterMultiplier * 2 :
            _characterMultiplier * 3;

        private int _characterMultiplier = 1;

        public bool HurryUp = false;

        public TextArchitect(TextMeshProUGUI tmProUI)
        {
            _tmProUI = tmProUI;
        }
        
        public TextArchitect(TextMeshPro tmProWorld)
        {
            _tmProWorld = tmProWorld;
        }

        public Coroutine Build(string text)
        {
            PreText = "";
            TargetText = text;
            
            Stop();

            _buildProcess = TmPro.StartCoroutine(Building());
            
            return _buildProcess;
        }
        
        public Coroutine Append(string text)
        {
            PreText = TmPro.text;
            TargetText = text;
            
            Stop();

            _buildProcess = TmPro.StartCoroutine(Building());
            
            return _buildProcess;
        }

        private Coroutine _buildProcess = null;
        public bool IsBuilding => _buildProcess != null;

        public void Stop()
        {
            if (!IsBuilding)
                return;
            
            TmPro.StopCoroutine(_buildProcess);
            _buildProcess = null;
        }

        private void Prepare()
        {
            switch (BuildMethodChosen)
            {
                case EBuildMethod.Instant:
                    PrepareInstant();
                    break;
                case EBuildMethod.Typewriter:
                    PrepareTypeWriter();
                    break;
                case EBuildMethod.Fade:
                    PrepareFade();
                    break;
            }
        }

        private IEnumerator Building()
        {
            Prepare();

            switch (BuildMethodChosen)
            {
                case EBuildMethod.Instant:
                    break;
                case EBuildMethod.Typewriter:
                    yield return BuildTypeWriter();
                    break;
                case EBuildMethod.Fade:
                    yield return BuildFade();
                    break;
            }
            
            OnComplete();
        }
        
        private void OnComplete()
        {
            _buildProcess = null;
            HurryUp = false;
        }

        public void ForceComplete()
        {
            switch (BuildMethodChosen)
            {
                case EBuildMethod.Typewriter:
                    TmPro.maxVisibleCharacters = TmPro.textInfo.characterCount;
                    break;
                case EBuildMethod.Fade:
                    break;
            }
            
            Stop();
            OnComplete();
        }
        
        private void PrepareInstant()
        {
            TmPro.color = TmPro.color;
            TmPro.text = FullTargetText;
            TmPro.ForceMeshUpdate();
            TmPro.maxVisibleCharacters = TmPro.textInfo.characterCount;
        }
        
        private void PrepareTypeWriter()
        {
            TmPro.color = TmPro.color;
            TmPro.maxVisibleCharacters = 0;
            TmPro.text = PreText;

            if (PreText != "")
            {
                TmPro.ForceMeshUpdate();
                TmPro.maxVisibleCharacters = TmPro.textInfo.characterCount;
            }

            TmPro.text += TargetText;
            TmPro.ForceMeshUpdate();
        }

        private void PrepareFade()
        {
            
        }
        
        private IEnumerator BuildTypeWriter()
        {
            while (TmPro.maxVisibleCharacters < TmPro.textInfo.characterCount)
            {
                TmPro.maxVisibleCharacters += HurryUp ? CharacterPorCycle * 5 : CharacterPorCycle;

                yield return new WaitForSeconds(0.015f / Speed);
            }
        }
        private IEnumerator BuildFade()
        {
            yield return null;
        }
    }
}