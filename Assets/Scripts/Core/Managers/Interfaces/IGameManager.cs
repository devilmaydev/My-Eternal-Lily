using Core.Characters;
using Core.Commands;
using Core.Dialogue;
using Core.Systems.Commands;
using Core.Systems.Dialogue;
using Core.Systems.Input;
using UnityEngine;

namespace Core.Managers.Interfaces
{
    /// <summary>
    /// Interface para o GameManager central
    /// Define contratos para gerenciamento de todos os sistemas
    /// </summary>
    public interface IGameManager
    {
        // Game State
        bool IsInitialized { get; }
        bool IsGameRunning { get; }
        
        // Managers Access
        AudioManager AudioManager { get; }
        InputManager InputManager { get; }
        CharacterManager CharacterManager { get; }
        DialogueSystem DialogueSystem { get; }
        CommandManager CommandManager { get; }
        
        // Game Control
        void InitializeGame();
        void StartGame();
        void PauseGame();
        void ResumeGame();
        void StopGame();
        
        // Manager Management
        T GetManager<T>() where T : MonoBehaviour;
        bool HasManager<T>() where T : MonoBehaviour;
    }
}
