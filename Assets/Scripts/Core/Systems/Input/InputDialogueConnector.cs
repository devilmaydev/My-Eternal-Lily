using UnityEngine;
using Core.Dialogue;
using Core.Systems.Dialogue;
using Core.Systems.Input.Events;

namespace Core.Systems.Input
{
    /// <summary>
    /// Conecta o novo sistema de input com o DialogueSystem
    /// Mantém compatibilidade sem legado
    /// </summary>
    public class InputDialogueConnector : MonoBehaviour
    {
        private void OnEnable()
        {
            // Subscribe to input events
            InputEvents.OnNextPressed += OnNextPressed;
            InputEvents.OnPreviousPressed += OnPreviousPressed;
            InputEvents.OnSkipPressed += OnSkipPressed;
            InputEvents.OnMenuPressed += OnMenuPressed;
            InputEvents.OnAutoPressed += OnAutoPressed;
            InputEvents.OnMouseClicked += OnMouseClicked;
        }
        
        private void OnDisable()
        {
            // Unsubscribe from input events
            InputEvents.OnNextPressed -= OnNextPressed;
            InputEvents.OnPreviousPressed -= OnPreviousPressed;
            InputEvents.OnSkipPressed -= OnSkipPressed;
            InputEvents.OnMenuPressed -= OnMenuPressed;
            InputEvents.OnAutoPressed -= OnAutoPressed;
            InputEvents.OnMouseClicked -= OnMouseClicked;
        }
        
        private void OnNextPressed()
        {
            if (DialogueSystem.Instance != null)
            {
                DialogueSystem.Instance.OnUserPromptNext();
            }
        }
        
        private void OnPreviousPressed()
        {
            // Implement previous functionality if needed
            Debug.Log("InputDialogueConnector: Previous pressed");
        }
        
        private void OnSkipPressed()
        {
            // Implement skip functionality if needed
            Debug.Log("InputDialogueConnector: Skip pressed");
        }
        
        private void OnMenuPressed()
        {
            // Implement menu functionality if needed
            Debug.Log("InputDialogueConnector: Menu pressed");
        }
        
        private void OnAutoPressed()
        {
            // Implement auto functionality if needed
            Debug.Log("InputDialogueConnector: Auto pressed");
        }
        
        private void OnMouseClicked(Vector2 position)
        {
            // Implement mouse click functionality if needed
            Debug.Log($"InputDialogueConnector: Mouse clicked at {position}");
        }
    }
}
