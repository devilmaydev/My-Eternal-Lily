using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue
{
    public class DialogueContinuePrompt : MonoBehaviour
    {
        private RectTransform Root;

        [SerializeField] private Animator anim;
        [SerializeField] private TextMeshProUGUI tmPro;

        public bool IsShowing => anim.gameObject.activeSelf;

        private void Start() => Root = GetComponent<RectTransform>();

        public void Show()
        {
            if (tmPro.text == string.Empty)
            {
                if (IsShowing)
                    Hide();

                return;
            }

            tmPro.ForceMeshUpdate();

            anim.gameObject.SetActive(true);
            Root.transform.SetParent(tmPro.transform);

            var finalCharacter = tmPro.textInfo.characterInfo[tmPro.textInfo.characterCount - 1];
            var targetPos = finalCharacter.bottomRight;
            var characterWidth = finalCharacter.pointSize * 0.5f;
            targetPos = new Vector3(targetPos.x + characterWidth, targetPos.y, 0);

            Root.localPosition = targetPos;
        }

        public void Hide() => anim.gameObject.SetActive(false);
    }
}
