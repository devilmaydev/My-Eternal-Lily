using System.Collections.Generic;
using UnityEngine;
using Core.Utils.IO;

namespace Core.Utils.IO
{
    /// <summary>
    /// Testa o carregamento de arquivos de diálogo
    /// Usado para diagnosticar problemas de carregamento
    /// </summary>
    public class DialogueFileTester : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private string testFileName = "A1C1.txt";
        [SerializeField] private bool testOnStart = true;
        
        private void Start()
        {
            if (testOnStart)
            {
                TestDialogueFileLoading();
            }
        }
        
        /// <summary>
        /// Testa o carregamento de arquivos de diálogo
        /// </summary>
        [ContextMenu("Test Dialogue File Loading")]
        public void TestDialogueFileLoading()
        {
            Debug.Log("=== DIALOGUE FILE LOADING TEST ===");
            
            // Teste 1: Verificar se o arquivo existe nos Resources
            TestResourceLoading();
            
            // Teste 2: Verificar FileManager.ReadTextAsset
            TestFileManager();
            
            // Teste 3: Verificar FilePaths
            TestFilePaths();
            
            Debug.Log("=== TEST COMPLETED ===");
        }
        
        private void TestResourceLoading()
        {
            Debug.Log("--- Test 1: Resources.Load ---");
            
            string resourcePath = FilePaths.ResourcesDialogueFiles + testFileName;
            Debug.Log($"Trying to load: {resourcePath}");
            
            var textAsset = Resources.Load<TextAsset>(resourcePath);
            
            if (textAsset != null)
            {
                Debug.Log($"✅ SUCCESS: Loaded '{testFileName}' from Resources");
                Debug.Log($"Text length: {textAsset.text.Length} characters");
                Debug.Log($"First 100 chars: {textAsset.text.Substring(0, Mathf.Min(100, textAsset.text.Length))}...");
            }
            else
            {
                Debug.LogError($"❌ FAILED: Could not load '{testFileName}' from Resources");
                Debug.LogError($"Resource path: {resourcePath}");
                
                // Listar arquivos disponíveis
                ListAvailableDialogueFiles();
            }
        }
        
        private void TestFileManager()
        {
            Debug.Log("--- Test 2: FileManager.ReadTextAsset ---");
            
            try
            {
                string resourcePath = FilePaths.ResourcesDialogueFiles + testFileName;
                var lines = FileManager.ReadTextAsset(resourcePath);
                
                if (lines != null && lines.Count > 0)
                {
                    Debug.Log($"✅ SUCCESS: FileManager loaded {lines.Count} lines");
                    Debug.Log($"First line: {lines[0]}");
                }
                else
                {
                    Debug.LogError($"❌ FAILED: FileManager returned null or empty lines");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ FAILED: FileManager threw exception: {ex.Message}");
            }
        }
        
        private void TestFilePaths()
        {
            Debug.Log("--- Test 3: FilePaths ---");
            
            Debug.Log($"FilePaths.ResourcesDialogueFiles: '{FilePaths.ResourcesDialogueFiles}'");
            Debug.Log($"FilePaths.Root: '{FilePaths.Root}'");
            
            string fullPath = FilePaths.ResourcesDialogueFiles + testFileName;
            Debug.Log($"Full resource path: '{fullPath}'");
        }
        
        private void ListAvailableDialogueFiles()
        {
            Debug.Log("--- Available Dialogue Files ---");
            
            try
            {
                // Teste 1: LoadAll com path específico
                var dialogueFiles = Resources.LoadAll<TextAsset>(FilePaths.ResourcesDialogueFiles);
                Debug.Log($"LoadAll with path '{FilePaths.ResourcesDialogueFiles}': {dialogueFiles.Length} files");
                
                if (dialogueFiles.Length > 0)
                {
                    Debug.Log($"Found {dialogueFiles.Length} dialogue files:");
                    foreach (var file in dialogueFiles)
                    {
                        Debug.Log($"  - {file.name}");
                    }
                }
                else
                {
                    Debug.LogWarning("No dialogue files found with LoadAll");
                    
                    // Teste 2: LoadAll sem path (todos os TextAssets)
                    var allTextAssets = Resources.LoadAll<TextAsset>("");
                    Debug.Log($"Total TextAssets in Resources: {allTextAssets.Length}");
                    
                    foreach (var asset in allTextAssets)
                    {
                        if (asset.name.Contains("A1C1") || asset.name.Contains("Dialogue"))
                        {
                            Debug.Log($"  - Found related asset: {asset.name}");
                        }
                    }
                    
                    // Teste 3: Verificar se pasta existe
                    Debug.Log("--- Testing different paths ---");
                    TestDifferentPaths();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error listing dialogue files: {ex.Message}");
            }
        }
        
        private void TestDifferentPaths()
        {
            string[] testPaths = {
                "DialogueFiles/",
                "DialogueFiles",
                "Resources/DialogueFiles/",
                "Resources/DialogueFiles",
                ""
            };
            
            foreach (string path in testPaths)
            {
                try
                {
                    var assets = Resources.LoadAll<TextAsset>(path);
                    Debug.Log($"Path '{path}': {assets.Length} TextAssets found");
                    
                    if (assets.Length > 0)
                    {
                        foreach (var asset in assets)
                        {
                            if (asset.name.Contains("A1C1"))
                            {
                                Debug.Log($"  - Found A1C1: {asset.name} at path '{path}'");
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error testing path '{path}': {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// Testa carregamento de arquivo específico
        /// </summary>
        public void TestSpecificFile(string fileName)
        {
            testFileName = fileName;
            TestDialogueFileLoading();
        }
    }
}
