using UnityEngine;
using Core.Systems.Input.Events;
using Core.Systems.Dialogue.Events;

namespace Core.Systems.Dialogue
{
    /// <summary>
    /// Conecta o sistema de diálogo com o sistema de input
    /// Mantém compatibilidade e integração entre sistemas
    /// </summary>
    public class DialogueInputConnector : MonoBehaviour
    {
        private void OnEnable()
        {
            // Subscribe to input events
            InputEvents.OnNextPressed += OnNextPressed;
            InputEvents.OnSkipPressed += OnSkipPressed;
            InputEvents.OnAutoPressed += OnAutoPressed;
        }
        
        private void OnDisable()
        {
            // Unsubscribe from input events
            InputEvents.OnNextPressed -= OnNextPressed;
            InputEvents.OnSkipPressed -= OnSkipPressed;
            InputEvents.OnAutoPressed -= OnAutoPressed;
        }
        
        private void OnNextPressed()
        {
            if (DialogueSystem.Instance != null)
            {
                DialogueSystem.Instance.OnUserPromptNext();
            }
        }
        
        private void OnSkipPressed()
        {
            // Implement skip functionality if needed
            Debug.Log("DialogueInputConnector: Skip pressed");
        }
        
        private void OnAutoPressed()
        {
            // Implement auto functionality if needed
            Debug.Log("DialogueInputConnector: Auto pressed");
        }
    }
}
