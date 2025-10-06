using UnityEngine;
using UnityEngine.Audio;

namespace Core.Systems.Audio.Settings
{
    /// <summary>
    /// Cria automaticamente VnAudioSettings se não existir
    /// Usado para evitar falhas de inicialização
    /// </summary>
    [CreateAssetMenu(fileName = "VnAudioSettings", menuName = "Visual Novel/Audio Settings", order = 1)]
    public class VnAudioSettingsCreator : ScriptableObject
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
        /// Converte para VnAudioSettings
        /// </summary>
        public VnAudioSettings ToVnAudioSettings()
        {
            return new VnAudioSettings
            {
                musicMixer = this.musicMixer,
                sfxMixer = this.sfxMixer,
                voicesMixer = this.voicesMixer,
                masterVolume = this.masterVolume,
                musicVolume = this.musicVolume,
                sfxVolume = this.sfxVolume,
                voiceVolume = this.voiceVolume,
                maxAudioSources = this.maxAudioSources,
                initialPoolSize = this.initialPoolSize,
                trackTransitionSpeed = this.trackTransitionSpeed,
                defaultVolume = this.defaultVolume,
                defaultPitch = this.defaultPitch,
                defaultLoop = this.defaultLoop,
                spatialBlend = this.spatialBlend,
                musicPath = this.musicPath,
                sfxPath = this.sfxPath,
                voicePath = this.voicePath
            };
        }
    }
}
