using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Core.Systems.Audio;
using Core.Systems.Audio.Events;
using Core.Systems.Audio.Interfaces;
using Core.Systems.Audio.Pooling;
using Core.Systems.Audio.Settings;

namespace Core.Managers
{
    public class AudioManager : MonoBehaviour, IAudioService
    {
        private const string SfxParentName = "SFX";
        private const string SfxNameFormat = "SFX - [{0}]";

        public static AudioManager Instance { get; private set; }

        private readonly Dictionary<int, AudioChannel> _channels = new();

        [FormerlySerializedAs("_audioSettings")]
        [Header("Audio Settings")]
        [SerializeField] private VnAudioSettings audioSettings;

        // Public properties for easy access
        public AudioMixerGroup MusicMixer => audioSettings?.musicMixer;
        public AudioMixerGroup SFXMixer => audioSettings?.sfxMixer;
        public AudioMixerGroup VoicesMixer => audioSettings?.voicesMixer;
        public float TrackTransitionSpeed => audioSettings?.trackTransitionSpeed ?? 1f;

        private Transform _sfxRoot;
        private AudioSourcePool _audioSourcePool;

        // Events
        public event System.Action<AudioClip> OnSoundEffectPlayed;
        public event System.Action<AudioTrack> OnTrackStarted;
        public event System.Action<string> OnTrackStopped;
        public event System.Action<System.Exception> OnAudioError;

        private void Awake()
        {
            if (Instance is null)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                Instance = this;
                Initialize();
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        private void Initialize()
        {
            // Create default settings if not assigned
            if (audioSettings == null)
            {
                Debug.LogWarning("AudioManager: AudioSettings not assigned, creating default settings...");
                audioSettings = CreateDefaultSettings();
            }

            if (!audioSettings.ValidateSettings())
            {
                Debug.LogError("AudioManager: AudioSettings validation failed!");
                return;
            }

            // Initialize audio source pool
            _audioSourcePool = gameObject.AddComponent<AudioSourcePool>();
            
            // Create SFX root
            _sfxRoot = new GameObject(SfxParentName).transform;
            _sfxRoot.SetParent(transform);

            // Apply volume settings
            audioSettings.ApplyVolumeSettings();
        }

        /// <summary>
        /// Cria configurações padrão para evitar falhas de inicialização
        /// </summary>
        private VnAudioSettings CreateDefaultSettings()
        {
            Debug.Log("AudioManager: Creating default audio settings...");
            
            return new VnAudioSettings
            {
                musicMixer = null, // Será configurado depois
                sfxMixer = null,   // Será configurado depois
                voicesMixer = null, // Será configurado depois
                masterVolume = 1f,
                musicVolume = 0.7f,
                sfxVolume = 1f,
                voiceVolume = 1f,
                maxAudioSources = 50,
                initialPoolSize = 10,
                trackTransitionSpeed = 1f,
                defaultVolume = 1f,
                defaultPitch = 1f,
                defaultLoop = false,
                spatialBlend = 0f,
                musicPath = "Audio/Music/",
                sfxPath = "Audio/SFX/",
                voicePath = "Audio/Voice/"
            };
        }

        public AudioSource PlaySoundEffect(string filePath, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
        {
            try
            {
                var clip = Resources.Load<AudioClip>(filePath);

                if (clip is not null) return PlaySoundEffect(clip, mixer, volume, pitch, loop);
                
                Debug.LogError($"Could not load audio file '{filePath}'. Please make sure this exists in the Resources directory!");
                AudioEvents.InvokeAudioFileNotFound(filePath);
                return null;

            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error playing sound effect '{filePath}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
                return null;
            }
        }

        public AudioSource PlaySoundEffect(AudioClip clip, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
        {
            try
            {
                // Validate parameters
                if (clip is null)
                {
                    Debug.LogError("AudioManager: AudioClip is null!");
                    return null;
                }

                if (volume is < 0 or > 1)
                {
                    Debug.LogWarning($"AudioManager: Volume {volume} is out of range [0,1]. Clamping to valid range.");
                    volume = Mathf.Clamp01(volume);
                }

                if (pitch <= 0)
                {
                    Debug.LogWarning($"AudioManager: Pitch {pitch} must be positive. Setting to 1.");
                    pitch = 1f;
                }

                // Get AudioSource from pool
                var effectSource = _audioSourcePool.GetAudioSource();
                effectSource.transform.SetParent(_sfxRoot);
                effectSource.transform.position = _sfxRoot.position;

                // Configure AudioSource
                effectSource.clip = clip;
                effectSource.outputAudioMixerGroup = mixer ?? audioSettings.sfxMixer;
                effectSource.volume = volume * audioSettings.sfxVolume;
                effectSource.spatialBlend = audioSettings.spatialBlend;
                effectSource.pitch = pitch;
                effectSource.loop = loop;

                // Play the sound
                effectSource.Play();

                // Invoke event
                OnSoundEffectPlayed?.Invoke(clip);
                AudioEvents.InvokeSoundEffectPlayed(clip);

                // Auto-destroy for non-looping sounds
                if (!loop)
                {
                    StartCoroutine(DestroyAfterPlayback(effectSource, clip.length / pitch));
                }

                return effectSource;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error playing sound effect '{clip.name}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
                return null;
            }
        }

        public AudioSource PlayVoice(string filePath, float volume = 1, float pitch = 1, bool loop = false)
        {
            try
            {
                var clip = Resources.Load<AudioClip>(filePath);

                if (clip is not null) return PlayVoice(clip, volume, pitch, loop);
                
                Debug.LogError($"Could not load voice file '{filePath}'. Please make sure this exists in the Resources directory!");
                AudioEvents.InvokeAudioFileNotFound(filePath);
                return null;

            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error playing voice '{filePath}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
                return null;
            }
        }

        public AudioSource PlayVoice(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false)
        {
            try
            {
                // Validate parameters
                if (clip is null)
                {
                    Debug.LogError("AudioManager: AudioClip is null!");
                    return null;
                }

                // Get AudioSource from pool
                var voiceSource = _audioSourcePool.GetAudioSource();
                voiceSource.transform.SetParent(_sfxRoot);
                voiceSource.transform.position = _sfxRoot.position;

                // Configure AudioSource
                voiceSource.clip = clip;
                voiceSource.outputAudioMixerGroup = audioSettings.voicesMixer;
                voiceSource.volume = volume * audioSettings.voiceVolume;
                voiceSource.spatialBlend = audioSettings.spatialBlend;
                voiceSource.pitch = pitch;
                voiceSource.loop = loop;

                // Play the voice
                voiceSource.Play();

                // Invoke event
                AudioEvents.InvokeVoicePlayed(clip);

                // Auto-destroy for non-looping voices
                if (!loop)
                {
                    StartCoroutine(DestroyAfterPlayback(voiceSource, clip.length / pitch));
                }

                return voiceSource;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error playing voice '{clip.name}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
                return null;
            }
        }

        public void StopSoundEffect(AudioClip clip) => StopSoundEffect(clip.name);

        public void StopSoundEffect(string soundName)
        {
            try
            {
                soundName = soundName.ToLower();

                var sources = _sfxRoot.GetComponentsInChildren<AudioSource>();
                foreach (var source in sources)
                {
                    if (source.clip is null || source.clip.name.ToLower() != soundName) continue;
                    source.Stop();
                    _audioSourcePool.ReturnAudioSource(source);
                    AudioEvents.InvokeSoundEffectStopped(soundName);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error stopping sound effect '{soundName}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
            }
        }

        /// <summary>
        /// Coroutine para destruir AudioSource após o playback
        /// </summary>
        private System.Collections.IEnumerator DestroyAfterPlayback(AudioSource source, float duration)
        {
            yield return new WaitForSeconds(duration);
            
            if (source is not null)
            {
                _audioSourcePool.ReturnAudioSource(source);
            }
        }

        public AudioTrack PlayTrack(string filePath, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f)
        {
            try
            {
                var clip = Resources.Load<AudioClip>(filePath);

                if (clip is not null) 
                    return PlayTrack(clip, channel, loop, startingVolume, volumeCap, pitch, filePath);
                
                Debug.LogError($"Could not load audio file '{filePath}'. Please make sure this exists in the Resources directory!");
                AudioEvents.InvokeAudioFileNotFound(filePath);
                return null;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error playing track '{filePath}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
                return null;
            }
        }

        public AudioTrack PlayTrack(AudioClip clip, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f, string filePath = "")
        {
            try
            {
                // Validate parameters
                if (clip is null)
                {
                    Debug.LogError("AudioManager: AudioClip is null!");
                    return null;
                }

                if (startingVolume < 0 || startingVolume > 1)
                {
                    Debug.LogWarning($"AudioManager: Starting volume {startingVolume} is out of range [0,1]. Clamping to valid range.");
                    startingVolume = Mathf.Clamp01(startingVolume);
                }

                if (volumeCap < 0 || volumeCap > 1)
                {
                    Debug.LogWarning($"AudioManager: Volume cap {volumeCap} is out of range [0,1]. Clamping to valid range.");
                    volumeCap = Mathf.Clamp01(volumeCap);
                }

                if (pitch <= 0)
                {
                    Debug.LogWarning($"AudioManager: Pitch {pitch} must be positive. Setting to 1.");
                    pitch = 1f;
                }

                var audioChannel = GetOrCreateChannel(channel);
                var track = audioChannel.PlayTrack(clip, loop, startingVolume, volumeCap, pitch, filePath);
                
                // Invoke event
                OnTrackStarted?.Invoke(track);
                AudioEvents.InvokeTrackStarted(track);
                
                return track;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error playing track '{clip.name}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
                return null;
            }
        }

        public void StopTrack(int channel)
        {
            try
            {
                var audioChannel = GetChannel(channel);

                if (audioChannel is null)
                {
                    Debug.LogWarning($"AudioManager: Channel {channel} not found!");
                    return;
                }

                audioChannel.StopTrack();
                AudioEvents.InvokeChannelStopped(channel);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error stopping track on channel {channel}: {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
            }
        }

        public void StopTrack(string trackName)
        {
            try
            {
                trackName = trackName.ToLower();

                foreach (var channel in _channels.Values)
                {
                    if (channel.ActiveTrack != null && channel.ActiveTrack.Name.ToLower() == trackName)
                    {
                        channel.StopTrack();
                        OnTrackStopped?.Invoke(trackName);
                        AudioEvents.InvokeTrackStopped(channel.ActiveTrack);
                        return;
                    }
                }
                
                Debug.LogWarning($"AudioManager: Track '{trackName}' not found!");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error stopping track '{trackName}': {ex.Message}");
                AudioEvents.InvokeAudioError(ex);
            }
        }

        private AudioChannel TryGetChannel(int channelNumber, bool createIfDoesNotExist = false)
        {
            if (_channels.TryGetValue(channelNumber, out var channel))
                return channel;

            if (!createIfDoesNotExist) 
                return null;
            
            channel = new AudioChannel(channelNumber);
            _channels.Add(channelNumber, channel);
            AudioEvents.InvokeChannelCreated(channelNumber);
            return channel;
        }

        // Interface methods
        public AudioChannel GetChannel(int channelNumber)
        {
            return _channels.GetValueOrDefault(channelNumber);
        }

        public AudioChannel GetOrCreateChannel(int channelNumber)
        {
            return TryGetChannel(channelNumber, true);
        }
    }
}