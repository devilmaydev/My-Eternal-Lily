using UnityEngine;
using Core.Dialogue;
using Core.Dialogue.Managers;
using Core.Commands;
using Core.Characters;
using Core.Utils.IO;
using System.Collections;

namespace Core.Utils.IO
{
    /// <summary>
    /// Teste completo do sistema de diálogo
    /// Adicione este script a um GameObject na cena para testar
    /// </summary>
    public class DialogueSystemTest : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private string testFileName = "A1C1";
        [SerializeField] private bool runTestOnStart = false;
        
        private void Start()
        {
            if (runTestOnStart)
            {
                StartCoroutine(RunCompleteTest());
            }
        }
        
        /// <summary>
        /// Executa teste completo do sistema
        /// </summary>
        [ContextMenu("Run Complete Test")]
        public void RunCompleteTestMenu()
        {
            StartCoroutine(RunCompleteTest());
        }
        
        private IEnumerator RunCompleteTest()
        {
            Debug.Log("=== DIALOGUE SYSTEM COMPLETE TEST ===");
            
            // Aguardar um frame para garantir inicialização
            yield return null;
            
            // Teste 1: Verificar Managers
            Debug.Log("--- Test 1: Checking Managers ---");
            if (!CheckManagers())
            {
                Debug.LogError("❌ Managers check failed!");
                yield break;
            }
            Debug.Log("✅ All managers are available");
            
            // Teste 2: Carregar arquivo
            Debug.Log("--- Test 2: Loading File ---");
            string filePath = FilePaths.GetPathToResource(FilePaths.ResourcesDialogueFiles, testFileName);
            var lines = FileManager.ReadTextAsset(filePath, includeBlankLines: true);
            
            if (lines == null || lines.Count == 0)
            {
                Debug.LogError($"❌ Failed to load file: {testFileName}");
                yield break;
            }
            Debug.Log($"✅ File loaded: {lines.Count} lines");
            Debug.Log($"First line: {lines[0]}");
            
            // Teste 3: Criar Conversation
            Debug.Log("--- Test 3: Creating Conversation ---");
            var conversation = new Conversation(lines);
            if (conversation == null)
            {
                Debug.LogError("❌ Failed to create Conversation");
                yield break;
            }
            Debug.Log("✅ Conversation created");
            Debug.Log($"HasReachedEnd: {conversation.HasReachedEnd()}");
            Debug.Log($"Progress: {conversation.GetProgress()}");
            
            if (!conversation.HasReachedEnd())
            {
                var currentLine = conversation.CurrentLine();
                Debug.Log($"CurrentLine: '{currentLine}'");
            }
            
            // Teste 4: Iniciar Conversation
            Debug.Log("--- Test 4: Starting Conversation ---");
            var coroutine = DialogueSystem.Instance.ConversationManager.StartConversation(conversation);
            
            if (coroutine == null)
            {
                Debug.LogError("❌ Failed to start conversation");
                yield break;
            }
            Debug.Log("✅ Conversation started");
            
            // Aguardar um pouco
            yield return new WaitForSeconds(0.1f);
            
            // Teste 5: Verificar estado
            Debug.Log("--- Test 5: Checking State ---");
            Debug.Log($"IsRunning: {DialogueSystem.Instance.ConversationManager.IsRunning}");
            Debug.Log($"Conversation exists: {DialogueSystem.Instance.ConversationManager.Conversation != null}");
            Debug.Log($"Progress: {DialogueSystem.Instance.ConversationManager.ConversationProgress}");
            
            if (DialogueSystem.Instance.ConversationManager.IsRunning)
            {
                Debug.Log("✅ Conversation is running successfully!");
            }
            else
            {
                Debug.LogWarning("⚠️ Conversation not running - may have finished or encountered an issue");
            }
            
            Debug.Log("=== DIALOGUE SYSTEM TEST COMPLETED ===");
        }
        
        /// <summary>
        /// Verifica se todos os managers estão disponíveis
        /// </summary>
        private bool CheckManagers()
        {
            if (DialogueSystem.Instance == null)
            {
                Debug.LogError("❌ DialogueSystem.Instance is null!");
                return false;
            }
            
            if (CommandManager.Instance == null)
            {
                Debug.LogError("❌ CommandManager.Instance is null!");
                return false;
            }
            
            if (CharacterManager.Instance == null)
            {
                Debug.LogError("❌ CharacterManager.Instance is null!");
                return false;
            }
            
            if (DialogueSystem.Instance.ConversationManager == null)
            {
                Debug.LogError("❌ ConversationManager is null!");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Testa comando load diretamente
        /// </summary>
        [ContextMenu("Test Load Command")]
        public void TestLoadCommand()
        {
            Debug.Log("=== TESTING LOAD COMMAND ===");
            
            if (CommandManager.Instance == null)
            {
                Debug.LogError("❌ CommandManager not available");
                return;
            }
            
            string[] args = { "-f", testFileName };
            Debug.Log($"Executing: load {string.Join(" ", args)}");
            
            var result = CommandManager.Instance.Execute("load", args);
            
            if (result != null)
            {
                Debug.Log("✅ Load command executed");
            }
            else
            {
                Debug.LogError("❌ Load command failed");
            }
        }
    }
}
