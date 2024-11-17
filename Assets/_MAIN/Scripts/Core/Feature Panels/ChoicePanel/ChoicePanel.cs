using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _MAIN.Scripts.Core.Feature_Panels.ChoicePanel
{
    public class ChoicePanel : MonoBehaviour
    {
        public static ChoicePanel Instance { get; private set; }

        private const float ButtonMinWidth = 50;
        private const float ButtonMaxWidth = 1000;
        private const float ButtonWidthPadding = 25;

        private const float ButtonHeightPerLine = 50;
        private const float ButtonHeightPadding = 20;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private GameObject choiceButtonPrefab;
        [SerializeField] private VerticalLayoutGroup buttonLayoutGroup;

        private CanvasGroupController _cg;
        private List<ChoiceButton> _buttons = new();
        public ChoicePanelDecision LastDecision { get; private set; }

        public bool IsWaitingOnUserChoice { get; private set; }

        private void Awake() => Instance = this;
        
        private void Start()
        {
            _cg = new CanvasGroupController(this, canvasGroup);

            _cg.Alpha = 0;
            _cg.SetInteractableState(false);
        }

        public void Show(string question, string[] choices)
        {
            LastDecision = new ChoicePanelDecision(question, choices);

            IsWaitingOnUserChoice = true;

            titleText.text = question;
            StartCoroutine(GenerateChoices(choices));
            
            _cg.Show();
            _cg.SetInteractableState(active: true);
        }

        private IEnumerator GenerateChoices(string[] choices)
        {
            float maxWidth = 0;

            for (int i = 0; i < choices.Length; i++)
            {
                ChoiceButton choiceButton;
                if (i < _buttons.Count)
                {
                    choiceButton = _buttons[i];
                }
                else
                {
                    var newButtonObject = Instantiate(choiceButtonPrefab, buttonLayoutGroup.transform);
                    newButtonObject.SetActive(true);

                    var newButton = newButtonObject.GetComponent<Button>();
                    var newTitle = newButton.GetComponentInChildren<TextMeshProUGUI>();
                    var newLayout = newButton.GetComponent<LayoutElement>();

                    choiceButton = new ChoiceButton { Button = newButton, Layout = newLayout, Title = newTitle };

                    _buttons.Add(choiceButton);
                }

                choiceButton.Button.onClick.RemoveAllListeners();
                var buttonIndex = i;
                choiceButton.Button.onClick.AddListener(() => AcceptAnswer(buttonIndex));
                choiceButton.Title.text = choices[i];

                var buttonWidth = Mathf.Clamp(ButtonWidthPadding + choiceButton.Title.preferredWidth, ButtonMinWidth, ButtonMaxWidth);
                maxWidth = Mathf.Max(maxWidth, buttonWidth);        
            }

            foreach (var button in _buttons)
            {
                button.Layout.preferredWidth = maxWidth;
            }

            for (int i = 0; i < _buttons.Count; i++)
            {
                var show = i < choices.Length;
                _buttons[i].Button.gameObject.SetActive(show);
            }

            yield return new WaitForEndOfFrame();

            foreach (var button in _buttons)
            {
                var lines = button.Title.textInfo.lineCount;
                button.Layout.preferredHeight = ButtonHeightPadding + (ButtonHeightPerLine * lines);
            }
        }

        public void Hide()
        {
            _cg.Hide();
            _cg.SetInteractableState(false);
        }

        private void AcceptAnswer(int index)
        {
            if (index < 0 || index > LastDecision.Choices.Length - 1)
                return;

            LastDecision.AnswerIndex = index;
            IsWaitingOnUserChoice = false;
            Hide();
        }
    }
}
