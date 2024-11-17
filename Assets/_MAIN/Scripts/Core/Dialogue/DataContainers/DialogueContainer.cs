using UnityEngine;
using TMPro;

namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;
        
        private CanvasGroupController _cgController;
        
        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontSize(float size) => dialogueText.fontSize = size;

        private bool _isInitialized = false;
        public void Initialize()
        {
            if (_isInitialized)
                return;

            _cgController = new CanvasGroupController(DialogueSystem.Instance, root.GetComponent<CanvasGroup>());
        }

        public bool IsVisible => _cgController.IsVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => _cgController.Show(speed, immediate);
        public Coroutine Hide(float speed = 1f, bool immediate = false) => _cgController.Hide(speed, immediate);
        
    }
}
