using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Systems.Input.Settings
{
    /// <summary>
    /// Configurações do sistema de input
    /// Centraliza todas as configurações de input
    /// </summary>
    [CreateAssetMenu(fileName = "VnInputSettings", menuName = "Visual Novel/Input Settings")]
    public class VnInputSettings : ScriptableObject
    {
        [Header("Input System Settings")]
        public bool enableInputSystem = true;
        public bool enableMouseInput = true;
        
        
        [Header("Mouse Settings")]
        public bool enableMouseClick = true;
        public bool enableMouseRightClick = true;
        public bool enableMouseMovement = false;
        
        [Header("Input System Actions")]
        public InputActionAsset inputActionAsset;
        public string nextActionName = "Next";
        public string previousActionName = "Previous";
        public string skipActionName = "Skip";
        public string menuActionName = "Menu";
        public string autoActionName = "Auto";
        public string settingsActionName = "Settings";
        public string quitActionName = "Quit";
        
        [Header("Input States")]
        public bool startWithInputEnabled = true;
        public bool startWithAutoMode = false;
        public bool startWithSkipMode = false;
        
        /// <summary>
        /// Valida as configurações
        /// </summary>
        public bool ValidateSettings()
        {
            if (inputActionAsset == null && enableInputSystem)
            {
                Debug.LogWarning("InputSettings: InputActionAsset is not assigned but Input System is enabled!");
                return false;
            }
            
            if (!enableInputSystem)
            {
                Debug.LogError("InputSettings: Input System is disabled!");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Aplica as configurações iniciais
        /// </summary>
        public void ApplyInitialSettings()
        {
            // Configurações iniciais serão aplicadas pelo InputManager
            Debug.Log("InputSettings: Initial settings applied");
        }
    }
}
