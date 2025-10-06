using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// Controlador do menu principal
    /// Gerencia navegação entre cenas e funcionalidades do menu
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [Header("Scene Settings")]
        [SerializeField] private string gameSceneName = "VisualNovel";
        [SerializeField] private float sceneTransitionDelay = 0.5f;
        
        [Header("Audio Settings")]
        [SerializeField] private string buttonClickSound = "Audio/SFX/button_click";
        [SerializeField] private string backgroundMusic = "Audio/Music/main_menu_theme";
        
        private void Start()
        {
            // Tocar música de fundo do menu
            PlayBackgroundMusic();
        }
        
        /// <summary>
        /// Inicia o jogo carregando a cena principal
        /// </summary>
        public void PlayGame()
        {
            Debug.Log("MainMenu: Iniciando jogo...");
            
            // Tocar som do botão
            PlayButtonSound();
            
            // Carregar cena do jogo
            StartCoroutine(LoadGameScene());
        }
        
        /// <summary>
        /// Sai do jogo
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("MainMenu: Saindo do jogo...");
            
            // Tocar som do botão
            PlayButtonSound();
            
            // Sair do jogo
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        /// <summary>
        /// Carrega a cena do jogo com delay
        /// </summary>
        private System.Collections.IEnumerator LoadGameScene()
        {
            // Aguardar delay
            yield return new WaitForSeconds(sceneTransitionDelay);
            
            // Carregar cena
            try
            {
                SceneManager.LoadScene(gameSceneName);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"MainMenu: Erro ao carregar cena '{gameSceneName}': {ex.Message}");
            }
        }
        
        /// <summary>
        /// Toca som de clique do botão
        /// </summary>
        private void PlayButtonSound()
        {
            if (!string.IsNullOrEmpty(buttonClickSound))
            {
                // Usar AudioManager se disponível
                if (Core.Managers.AudioManager.Instance != null)
                {
                    Core.Managers.AudioManager.Instance.PlaySoundEffect(buttonClickSound);
                }
                else
                {
                    Debug.LogWarning("MainMenu: AudioManager não disponível para tocar som do botão");
                }
            }
        }
        
        /// <summary>
        /// Toca música de fundo
        /// </summary>
        private void PlayBackgroundMusic()
        {
            if (!string.IsNullOrEmpty(backgroundMusic))
            {
                // Usar AudioManager se disponível
                if (Core.Managers.AudioManager.Instance != null)
                {
                    Core.Managers.AudioManager.Instance.PlayTrack(backgroundMusic, 0, true, 0f, 0.7f);
                }
                else
                {
                    Debug.LogWarning("MainMenu: AudioManager não disponível para tocar música de fundo");
                }
            }
        }
        
        /// <summary>
        /// Para a música de fundo
        /// </summary>
        public void StopBackgroundMusic()
        {
            if (Core.Managers.AudioManager.Instance != null)
            {
                Core.Managers.AudioManager.Instance.StopTrack(0);
            }
        }
    }
}
