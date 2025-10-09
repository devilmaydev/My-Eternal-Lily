using UnityEngine;

namespace Core.Systems.Input.Interfaces
{
    /// <summary>
    /// Interface para o sistema de input
    /// Define contratos para gerenciamento de input
    /// </summary>
    public interface IInputService
    {
        // Events
        event System.Action OnNextPressed;
        event System.Action OnPreviousPressed;
        event System.Action OnSkipPressed;
        event System.Action OnMenuPressed;
        event System.Action OnAutoPressed;
        event System.Action<Vector2> OnMouseMoved;
        event System.Action<Vector2> OnMouseClicked;
        
        // Input State
        bool IsInputEnabled { get; set; }
        bool IsAutoMode { get; set; }
        bool IsSkipMode { get; set; }
        
        // Input Methods
        void EnableInput();
        void DisableInput();
        void ToggleAutoMode();
        void ToggleSkipMode();
        
        // Configuration
        void SetInputMapping(string actionName, KeyCode keyCode);
        void SetInputMapping(string actionName, string inputActionName);
        
        // Input Detection
        bool IsKeyPressed(KeyCode keyCode);
        bool IsKeyDown(KeyCode keyCode);
        bool IsKeyUp(KeyCode keyCode);
        Vector2 GetMousePosition();
        bool IsMouseButtonDown(int button);
    }
}
