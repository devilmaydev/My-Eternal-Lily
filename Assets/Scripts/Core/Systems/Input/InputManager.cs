using UnityEngine;
using UnityEngine.InputSystem;
using Core.Systems.Input.Interfaces;
using Core.Systems.Input.Events;
using Core.Systems.Input.Settings;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Core.Systems.Input
{
    /// <summary>
    /// Manager moderno do sistema de input
    /// Sistema unificado com Input System e configuração flexível
    /// </summary>
    public class InputManager : MonoBehaviour, IInputService
    {
        [Header("Input Settings")]
        [SerializeField] private VnInputSettings inputSettings;
        
        [Header("Input System Components")]
        [SerializeField] private PlayerInput playerInput;
        
        // Singleton
        public static InputManager Instance { get; private set; }
        
        // Input State
        public bool IsInputEnabled { get; set; } = true;
        public bool IsAutoMode { get; set; } = false;
        public bool IsSkipMode { get; set; } = false;
        
        // Events
        public event System.Action OnNextPressed;
        public event System.Action OnPreviousPressed;
        public event System.Action OnSkipPressed;
        public event System.Action OnMenuPressed;
        public event System.Action OnAutoPressed;
        public event System.Action<Vector2> OnMouseMoved;
        public event System.Action<Vector2> OnMouseClicked;
        
        // Input System Actions
        private readonly Dictionary<string, InputAction> _inputActions = new();
        private Vector2 _lastMousePosition;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeInputSystem();
                ApplyInitialSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Subscribe to InputEvents for global input handling
            SubscribeToInputEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromInputEvents();
        }
        
        private void Update()
        {
            if (!IsInputEnabled) return;
            
            HandleMouseInput();
        }
        
        #region Initialization
        
        private void InitializeInputSystem()
        {
            if (!inputSettings.enableInputSystem || inputSettings.inputActionAsset == null)
            {
                Debug.Log("InputManager: Input System disabled or not configured");
                return;
            }
            
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                playerInput = gameObject.AddComponent<PlayerInput>();
            }
            
            playerInput.actions = inputSettings.inputActionAsset;
            playerInput.enabled = true;
            
            // Initialize input actions
            InitializeInputActions();
        }
        
        private void InitializeInputActions()
        {
            if (playerInput?.actions == null) return;
            
            var actions = playerInput.actions;
            
            // Map input actions
            MapInputAction(inputSettings.nextActionName, OnNextInput);
            MapInputAction(inputSettings.previousActionName, OnPreviousInput);
            MapInputAction(inputSettings.skipActionName, OnSkipInput);
            MapInputAction(inputSettings.menuActionName, OnMenuInput);
            MapInputAction(inputSettings.autoActionName, OnAutoInput);
            MapInputAction(inputSettings.settingsActionName, OnSettingsInput);
            MapInputAction(inputSettings.quitActionName, OnQuitInput);
        }
        
        private void MapInputAction(string actionName, System.Action<InputAction.CallbackContext> callback)
        {
            if (string.IsNullOrEmpty(actionName)) return;
            
            var action = playerInput.actions[actionName];
            
            if (action == null) return;
            
            action.performed += callback;
            _inputActions[actionName] = action;
        }
        
        
        private void ApplyInitialSettings()
        {
            IsInputEnabled = inputSettings.startWithInputEnabled;
            IsAutoMode = inputSettings.startWithAutoMode;
            IsSkipMode = inputSettings.startWithSkipMode;
        }
        
        #endregion
        
        #region Input Handling
        
        
        private void HandleMouseInput()
        {
            if (!inputSettings.enableMouseInput) return;
            
            Vector2 mousePosition = UnityEngine.Input.mousePosition;
            
            // Mouse movement
            if (inputSettings.enableMouseMovement && mousePosition != _lastMousePosition)
            {
                OnMouseMoved?.Invoke(mousePosition);
                InputEvents.InvokeMouseMoved(mousePosition);
                _lastMousePosition = mousePosition;
            }
            
            // Mouse clicks
            if (inputSettings.enableMouseClick && UnityEngine.Input.GetMouseButtonDown(0))
            {
                OnMouseClicked?.Invoke(mousePosition);
                InputEvents.InvokeMouseClicked(mousePosition);
            }
            
            if (inputSettings.enableMouseRightClick && UnityEngine.Input.GetMouseButtonDown(1))
            {
                InputEvents.InvokeMouseRightClicked(mousePosition);
            }
        }
        
        #endregion
        
        #region Input Callbacks
        
        private void OnNextInput(InputAction.CallbackContext context)
        {
            if (!IsInputEnabled) return;
            
            OnNextPressed?.Invoke();
            InputEvents.InvokeNextPressed();
        }
        
        private void OnPreviousInput(InputAction.CallbackContext context)
        {
            if (!IsInputEnabled) return;
            
            OnPreviousPressed?.Invoke();
            InputEvents.InvokePreviousPressed();
        }
        
        private void OnSkipInput(InputAction.CallbackContext context)
        {
            if (!IsInputEnabled) return;
            
            OnSkipPressed?.Invoke();
            InputEvents.InvokeSkipPressed();
        }
        
        private void OnMenuInput(InputAction.CallbackContext context)
        {
            if (!IsInputEnabled) return;
            
            OnMenuPressed?.Invoke();
            InputEvents.InvokeMenuPressed();
        }
        
        private void OnAutoInput(InputAction.CallbackContext context)
        {
            if (!IsInputEnabled) return;
            
            OnAutoPressed?.Invoke();
            InputEvents.InvokeAutoPressed();
            ToggleAutoMode();
        }
        
        private void OnSettingsInput(InputAction.CallbackContext context)
        {
            if (!IsInputEnabled) return;
            
            InputEvents.InvokeSettingsPressed();
        }
        
        private void OnQuitInput(InputAction.CallbackContext context)
        {
            if (!IsInputEnabled) return;
            
            InputEvents.InvokeQuitPressed();
        }
        
        #endregion
        
        #region IInputService Implementation
        
        public void EnableInput()
        {
            IsInputEnabled = true;
            InputEvents.InvokeInputEnabledChanged(true);
        }
        
        public void DisableInput()
        {
            IsInputEnabled = false;
            InputEvents.InvokeInputEnabledChanged(false);
        }
        
        public void ToggleAutoMode()
        {
            IsAutoMode = !IsAutoMode;
            InputEvents.InvokeAutoModeChanged(IsAutoMode);
        }
        
        public void ToggleSkipMode()
        {
            IsSkipMode = !IsSkipMode;
            InputEvents.InvokeSkipModeChanged(IsSkipMode);
        }
        
        public void SetInputMapping(string actionName, KeyCode keyCode)
        {
            // This would require a more complex implementation
            Debug.Log($"InputManager: Setting {actionName} to {keyCode}");
        }
        
        public void SetInputMapping(string actionName, string inputActionName)
        {
            // This would require a more complex implementation
            Debug.Log($"InputManager: Setting {actionName} to {inputActionName}");
        }
        
        public bool IsKeyPressed(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKey(keyCode);
        }
        
        public bool IsKeyDown(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyDown(keyCode);
        }
        
        public bool IsKeyUp(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyUp(keyCode);
        }
        
        public Vector2 GetMousePosition()
        {
            return UnityEngine.Input.mousePosition;
        }
        
        public bool IsMouseButtonDown(int button)
        {
            return UnityEngine.Input.GetMouseButtonDown(button);
        }
        
        #endregion
        
        #region Event Management
        
        private void SubscribeToInputEvents()
        {
            // Subscribe to global input events if needed
        }
        
        private void UnsubscribeFromInputEvents()
        {
            // Unsubscribe from global input events if needed
        }
        
        #endregion
    }
}
