using Core.Dialogue;
using UnityEngine;

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Return))
                PromptAdvance();
        }

        public void PromptAdvance()
        {
            DialogueSystem.Instance.OnUserPromptNext();
        }
    }
}
