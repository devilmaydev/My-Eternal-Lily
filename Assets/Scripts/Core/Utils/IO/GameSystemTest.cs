using UnityEngine;
using Core.Managers;
using Core.Systems.Dialogue;
using Core.Systems.Commands;
using Core.Systems.Input;
using Core.Systems.Audio;

namespace Core.Utils.IO
{
    /// <summary>
    /// Teste completo do sistema do jogo
    /// Verifica se todos os managers estão funcionando corretamente
    /// </summary>
    public class GameSystemTest : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private bool autoStart = true;

        private void Start()
        {
            if (autoStart)
            {
                StartCoroutine(RunSystemTest());
            }
        }

        [ContextMenu("Run System Test")]
        public void RunSystemTestMenu()
        {
            StartCoroutine(RunSystemTest());
        }

        private System.Collections.IEnumerator RunSystemTest()
        {
            Debug.Log("=== GAME SYSTEM TEST ===");

            // Aguardar inicialização
            yield return new WaitForSeconds(0.5f);

            // Teste 1: Verificar GameManager
            Debug.Log("--- Test 1: GameManager ---");
            if (GameManager.Instance == null)
            {
                Debug.LogError("❌ GameManager.Instance is null!");
                yield break;
            }
            Debug.Log("✅ GameManager.Instance is available.");

            // Teste 2: Verificar CharacterManager
            Debug.Log("--- Test 2: CharacterManager ---");
            if (GameManager.Instance.CharacterManager == null)
            {
                Debug.LogError("❌ CharacterManager is null in GameManager!");
                yield break;
            }
            Debug.Log("✅ CharacterManager is available.");

            // Teste 3: Verificar DialogueSystem
            Debug.Log("--- Test 3: DialogueSystem ---");
            if (GameManager.Instance.DialogueSystem == null)
            {
                Debug.LogError("❌ DialogueSystem is null in GameManager!");
                yield break;
            }
            Debug.Log("✅ DialogueSystem is available.");

            // Teste 4: Verificar CommandManager
            Debug.Log("--- Test 4: CommandManager ---");
            if (GameManager.Instance.CommandManager == null)
            {
                Debug.LogError("❌ CommandManager is null in GameManager!");
                yield break;
            }
            Debug.Log("✅ CommandManager is available.");

            // Teste 5: Verificar InputManager
            Debug.Log("--- Test 5: InputManager ---");
            if (GameManager.Instance.InputManager == null)
            {
                Debug.LogError("❌ InputManager is null in GameManager!");
                yield break;
            }
            Debug.Log("✅ InputManager is available.");

            // Teste 6: Verificar AudioManager
            Debug.Log("--- Test 6: AudioManager ---");
            if (GameManager.Instance.AudioManager == null)
            {
                Debug.LogError("❌ AudioManager is null in GameManager!");
                yield break;
            }
            Debug.Log("✅ AudioManager is available.");

            // Teste 7: Testar criação de personagem
            Debug.Log("--- Test 7: Character Creation ---");
            try
            {
                var character = GameManager.Instance.CharacterManager.CreateCharacter("TestCharacter");
                if (character == null)
                {
                    Debug.LogWarning("⚠️ Character creation returned null (this might be expected if character config doesn't exist)");
                }
                else
                {
                    Debug.Log("✅ Character created successfully.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Character creation failed: {e.Message}");
            }

            // Teste 8: Testar execução de comando
            Debug.Log("--- Test 8: Command Execution ---");
            try
            {
                var result = GameManager.Instance.CommandManager.Execute("help");
                if (result == null)
                {
                    Debug.LogWarning("⚠️ Command execution returned null (this might be expected if command doesn't exist)");
                }
                else
                {
                    Debug.Log("✅ Command executed successfully.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Command execution failed: {e.Message}");
            }

            Debug.Log("=== GAME SYSTEM TEST COMPLETED ===");
        }
    }
}
