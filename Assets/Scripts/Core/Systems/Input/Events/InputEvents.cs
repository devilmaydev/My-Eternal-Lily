using UnityEngine;

namespace Core.Systems.Input.Events
{
    /// <summary>
    /// Eventos centralizados do sistema de input
    /// Permite comunicação desacoplada entre sistemas
    /// </summary>
    public static class InputEvents
    {
        // Input Events
        public static event System.Action OnNextPressed;
        public static event System.Action OnPreviousPressed;
        public static event System.Action OnSkipPressed;
        public static event System.Action OnMenuPressed;
        public static event System.Action OnAutoPressed;
        public static event System.Action OnSettingsPressed;
        public static event System.Action OnQuitPressed;
        
        // Mouse Events
        public static event System.Action<Vector2> OnMouseMoved;
        public static event System.Action<Vector2> OnMouseClicked;
        public static event System.Action<Vector2> OnMouseRightClicked;
        
        // Input State Events
        public static event System.Action<bool> OnInputEnabledChanged;
        public static event System.Action<bool> OnAutoModeChanged;
        public static event System.Action<bool> OnSkipModeChanged;
        
        // Event Invokers
        public static void InvokeNextPressed() => OnNextPressed?.Invoke();
        public static void InvokePreviousPressed() => OnPreviousPressed?.Invoke();
        public static void InvokeSkipPressed() => OnSkipPressed?.Invoke();
        public static void InvokeMenuPressed() => OnMenuPressed?.Invoke();
        public static void InvokeAutoPressed() => OnAutoPressed?.Invoke();
        public static void InvokeSettingsPressed() => OnSettingsPressed?.Invoke();
        public static void InvokeQuitPressed() => OnQuitPressed?.Invoke();
        
        public static void InvokeMouseMoved(Vector2 position) => OnMouseMoved?.Invoke(position);
        public static void InvokeMouseClicked(Vector2 position) => OnMouseClicked?.Invoke(position);
        public static void InvokeMouseRightClicked(Vector2 position) => OnMouseRightClicked?.Invoke(position);
        
        public static void InvokeInputEnabledChanged(bool enabled) => OnInputEnabledChanged?.Invoke(enabled);
        public static void InvokeAutoModeChanged(bool autoMode) => OnAutoModeChanged?.Invoke(autoMode);
        public static void InvokeSkipModeChanged(bool skipMode) => OnSkipModeChanged?.Invoke(skipMode);
    }
}
