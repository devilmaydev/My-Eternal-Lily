using UnityEngine;

namespace Core.Systems.Commands.Settings
{
    /// <summary>
    /// Configurações do sistema de comandos
    /// Centraliza todas as configurações de comandos
    /// </summary>
    [CreateAssetMenu(fileName = "VnCommandSettings", menuName = "Visual Novel/Command Settings")]
    public class VnCommandSettings : ScriptableObject
    {
        [Header("Command Execution Settings")]
        public bool enableCommandLogging = true;
        public bool enableCommandValidation = true;
        public bool enableCommandTiming = false;
        public float commandTimeout = 30f;
        
        [Header("Process Management")]
        public int maxActiveProcesses = 10;
        public bool enableProcessQueue = true;
        public bool enableProcessPriority = false;
        public bool autoStopOnError = true;
        
        [Header("Database Settings")]
        public bool enableSubDatabases = true;
        public bool enableCharacterCommands = true;
        public bool enableCommandExtensions = true;
        public bool enableReflectionLoading = true;
        
        [Header("Character Command Settings")]
        public bool enableCharacterTypeRouting = true;
        public bool enableCharacterValidation = true;
        public bool enableCharacterFallback = true;
        
        [Header("Performance Settings")]
        public bool enableCommandCaching = true;
        public bool enableProcessPooling = false;
        public int commandCacheSize = 100;
        public int processPoolSize = 20;
        
        [Header("Debug Settings")]
        public bool enableDebugMode = false;
        public bool enableCommandTrace = false;
        public bool enableProcessTrace = false;
        public bool enableDatabaseTrace = false;
        
        /// <summary>
        /// Valida as configurações
        /// </summary>
        public bool ValidateSettings()
        {
            if (commandTimeout <= 0)
            {
                Debug.LogError("VnCommandSettings: Command timeout must be greater than 0!");
                return false;
            }
            
            if (maxActiveProcesses <= 0)
            {
                Debug.LogError("VnCommandSettings: Max active processes must be greater than 0!");
                return false;
            }
            
            if (commandCacheSize <= 0)
            {
                Debug.LogError("VnCommandSettings: Command cache size must be greater than 0!");
                return false;
            }
            
            if (processPoolSize <= 0)
            {
                Debug.LogError("VnCommandSettings: Process pool size must be greater than 0!");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Aplica as configurações iniciais
        /// </summary>
        public void ApplyInitialSettings()
        {
            // Configurações iniciais serão aplicadas pelo CommandManager
            Debug.Log("VnCommandSettings: Initial settings applied");
        }
    }
}
