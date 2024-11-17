using System.Collections;
using _MAIN.Scripts.Core.Dialogue.Managers;
using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue
{
    public class AutoReader : MonoBehaviour
    {
        private const int DefaultCharactersReadPerSecond = 18;
        private const float ReadTimePadding = 0.5f;
        private const float MaxReadTime = 99f;
        private const float MinReadTime = 1f;
        private const string StatusTextAuto = "Auto";
        private const string StatusTextSkip = "Skipping";

        private ConversationManager _conversationManager;
        private TextArchitect Architect => _conversationManager.TextArchitect;

        public bool Skip { get; set; }
        public float speed = 1f;

        public bool isOn => _coRunning != null;
        private Coroutine _coRunning;

        [SerializeField] private TextMeshProUGUI statusText;

        public void Initialize(ConversationManager conversationManager)
        {
            _conversationManager = conversationManager;

            statusText.text = string.Empty;
        }

        public void Enable()
        {
            if (isOn)
                return;

            _coRunning = StartCoroutine(AutoRead());
        }

        public void Disable()
        {
            if (!isOn) 
                return;

            StopCoroutine(_coRunning);
            Skip = false;
            _coRunning = null;
            statusText.text = string.Empty;
        }

        private IEnumerator AutoRead()
        {
            //Do nothing if there is no conversation to monitor.
            if (!_conversationManager.IsRunning)
            {
                Disable();
                yield break;
            }

            if (!Architect.IsBuilding && Architect.CurrentText != string.Empty)
                DialogueSystem.Instance.OnSystemPromptNext();

            while (_conversationManager.IsRunning)
            {
                //Read and wait
                if (!Skip)
                {
                    while (!Architect.IsBuilding && !_conversationManager.IsWaitingOnAutoTimer)
                        yield return null;

                    yield return new WaitForSeconds(0.02f);

                    float timeStarted = Time.time;

                    while (Architect.IsBuilding || _conversationManager.IsWaitingOnAutoTimer)
                        yield return null;

                    var timeToRead = Mathf.Clamp(((float)Architect.TmPro.textInfo.characterCount / DefaultCharactersReadPerSecond), MinReadTime, MaxReadTime);
                    timeToRead = Mathf.Clamp((timeToRead - (Time.time - timeStarted)), MinReadTime, MaxReadTime);
                    timeToRead = (timeToRead / speed) + ReadTimePadding;

                    Debug.Log($"wait [{timeToRead}s] for '{Architect.CurrentText}'");

                    yield return new WaitForSeconds(timeToRead);
                }
                //Skip
                else
                {
                    Architect.ForceComplete();
                    yield return new WaitForSeconds(0.05f);
                }

                DialogueSystem.Instance.OnSystemPromptNext();
            }

            Disable();
        }

        public void Toggle_Auto()
        {
            if (Skip)
                Enable();

            else
            {
                if (!isOn)
                    Enable();
                else
                    Disable();
            }

            Skip = false;

            if (isOn)
                statusText.text = StatusTextAuto;
        }

        public void Toggle_Skip()
        {
            if (!Skip)
                Enable();

            else
            {
                if (!isOn)
                    Enable();
                else
                    Disable();
            }

            Skip = true;

            if (isOn)
                statusText.text = StatusTextSkip;
        }
    }
}
