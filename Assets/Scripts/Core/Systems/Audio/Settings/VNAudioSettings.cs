using UnityEngine;
using UnityEngine.Audio;

namespace Core.Systems.Audio.Settings
{
    /// <summary>
    /// Configurações do sistema de áudio do Visual Novel Engine
    /// Centraliza todas as configurações de áudio
    /// </summary>
    [System.Serializable]
    public class VnAudioSettings
    {
        [Header("Audio Mixer Groups")]
        public AudioMixerGroup musicMixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup voicesMixer;
        
        [Header("Volume Settings")]
        [Range(0f, 1f)]
        public float masterVolume = 1f;
        
        [Range(0f, 1f)]
        public float musicVolume = 1f;
        
        [Range(0f, 1f)]
        public float sfxVolume = 1f;
        
        [Range(0f, 1f)]
        public float voiceVolume = 1f;
        
        [Header("Performance Settings")]
        public int maxAudioSources = 50;
        public int initialPoolSize = 10;
        public float trackTransitionSpeed = 1f;
        
        [Header("Audio Source Defaults")]
        public float defaultVolume = 1f;
        public float defaultPitch = 1f;
        public bool defaultLoop = false;
        public float spatialBlend = 0f; // 2D audio
        
        [Header("File Paths")]
        public string musicPath = "Audio/Music/";
        public string sfxPath = "Audio/SFX/";
        public string voicePath = "Audio/Voice/";
        
        /// <summary>
        /// Aplica as configurações de volume aos mixers
        /// </summary>
        public void ApplyVolumeSettings()
        {
            try
            {
                if (musicMixer != null && musicMixer.audioMixer != null)
                {
                    musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
                }
                
                if (sfxMixer != null && sfxMixer.audioMixer != null)
                {
                    sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
                }
                
                if (voicesMixer != null && voicesMixer.audioMixer != null)
                {
                    voicesMixer.audioMixer.SetFloat("VoiceVolume", Mathf.Log10(voiceVolume) * 20);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"VnAudioSettings: Could not apply volume settings - {ex.Message}");
            }
        }
        
        /// <summary>
        /// Valida as configurações
        /// </summary>
        public bool ValidateSettings()
        {
            if (musicMixer == null)
            {
                Debug.LogError("VnAudioSettings: Music Mixer is not assigned!");
                return false;
            }
            
            if (sfxMixer == null)
            {
                Debug.LogError("VnAudioSettings: SFX Mixer is not assigned!");
                return false;
            }
            
            if (voicesMixer == null)
            {
                Debug.LogError("VnAudioSettings: Voice Mixer is not assigned!");
                return false;
            }
            
            if (maxAudioSources <= 0)
            {
                Debug.LogError("VnAudioSettings: Max Audio Sources must be greater than 0!");
                return false;
            }
            
            if (initialPoolSize < 0)
            {
                Debug.LogError("VnAudioSettings: Initial Pool Size cannot be negative!");
                return false;
            }
            
            return true;
        }
    }
}
