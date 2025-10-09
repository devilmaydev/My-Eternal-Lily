using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Core.Managers.Interfaces;
using Core.Systems.Input;
using Core.Dialogue;
using Core.Characters;
using Core.Commands;
using Core.Systems.Commands;
using Core.Systems.Dialogue;
using UnityEngine.Serialization;

namespace Core.Managers
{
    /// <summary>
    /// GameManager central - Gerencia todos os managers do jogo
    /// Segue padrões AAA de desenvolvimento de jogos
    /// </summary>
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Header("Game Manager Settings")]
        [SerializeField] private bool debugMode = false;
        
        [Header("Manager References")]
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private CharacterManager characterManager;
        [SerializeField] private DialogueSystem dialogueSystem;
        [SerializeField] private CommandManager commandManager;
        
        // Singleton
        public static GameManager Instance { get; private set; }
        
        // Game State
        public bool IsInitialized { get; private set; } = false;
        public bool IsGameRunning { get; private set; } = false;
        
        // Managers Access (Properties)
        public AudioManager AudioManager => audioManager;
        public InputManager InputManager => inputManager;
        public CharacterManager CharacterManager => characterManager;
        public DialogueSystem DialogueSystem => dialogueSystem;
        public CommandManager CommandManager => commandManager;
        
        // Manager Registry
        private Dictionary<System.Type, MonoBehaviour> _managers = new Dictionary<System.Type, MonoBehaviour>();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        #region Game Control
        
        public void InitializeGame()
        {
            if (IsInitialized)
            {
                LogDebug("GameManager: Already initialized");
                return;
            }
            
            LogDebug("=== GAME MANAGER INITIALIZATION ===");
            
            // Register managers
            RegisterManager(audioManager);
            RegisterManager(inputManager);
            RegisterManager(characterManager);
            RegisterManager(commandManager);
            RegisterManager(dialogueSystem);
            
            IsInitialized = true;
            LogDebug("✅ GameManager initialization completed!");
        }
        
        public void StartGame()
        {
            if (!IsInitialized)
            {
                LogDebug("GameManager: Cannot start game - not initialized");
                return;
            }
            
            if (IsGameRunning)
            {
                LogDebug("GameManager: Game already running");
                return;
            }
            
            LogDebug("=== STARTING GAME ===");
            
            // Enable all managers
            EnableAllManagers();
            
            IsGameRunning = true;
            LogDebug("✅ Game started successfully!");
        }
        
        public void PauseGame()
        {
            if (!IsGameRunning) return;
            
            LogDebug("=== PAUSING GAME ===");
            
            // Pause game systems
            Time.timeScale = 0f;
            
            // Disable input
            if (inputManager != null)
            {
                inputManager.DisableInput();
            }
            
            LogDebug("✅ Game paused");
        }
        
        public void ResumeGame()
        {
            if (IsGameRunning) return;
            
            LogDebug("=== RESUMING GAME ===");
            
            // Resume game systems
            Time.timeScale = 1f;
            
            // Enable input
            if (inputManager != null)
            {
                inputManager.EnableInput();
            }
            
            IsGameRunning = true;
            LogDebug("✅ Game resumed");
        }
        
        public void StopGame()
        {
            if (!IsGameRunning) return;
            
            LogDebug("=== STOPPING GAME ===");
            
            // Disable all managers
            DisableAllManagers();
            
            IsGameRunning = false;
            LogDebug("✅ Game stopped");
        }
        
        #endregion
        
        #region Manager Management
        
        private void RegisterManager<T>(T manager) where T : MonoBehaviour
        {
            if (manager != null)
            {
                _managers[typeof(T)] = manager;
                LogDebug($"✅ {typeof(T).Name} registered");
            }
            else
            {
                LogDebug($"❌ {typeof(T).Name} is null - not registered");
            }
        }
        
        public T GetManager<T>() where T : MonoBehaviour
        {
            if (_managers.TryGetValue(typeof(T), out MonoBehaviour manager))
            {
                return manager as T;
            }
            
            LogDebug($"Manager {typeof(T).Name} not found");
            return null;
        }
        
        public bool HasManager<T>() where T : MonoBehaviour
        {
            return _managers.ContainsKey(typeof(T));
        }
        
        private void EnableAllManagers()
        {
            foreach (var manager in _managers.Values)
            {
                if (manager != null)
                {
                    manager.enabled = true;
                }
            }
        }
        
        private void DisableAllManagers()
        {
            foreach (var manager in _managers.Values)
            {
                if (manager != null)
                {
                    manager.enabled = false;
                }
            }
        }
        
        #endregion
        
        #region Utility
        
        private void LogDebug(string message)
        {
            if (debugMode)
            {
                Debug.Log($"[GameManager] {message}");
            }
        }
        
        #endregion
        
        #region Unity Events
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        
        #endregion
    }
}
