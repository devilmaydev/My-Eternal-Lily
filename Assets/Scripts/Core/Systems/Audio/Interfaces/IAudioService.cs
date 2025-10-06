using UnityEngine;
using UnityEngine.Audio;

namespace Core.Systems.Audio.Interfaces
{
    /// <summary>
    /// Interface para o serviço de áudio
    /// Seguindo o princípio de inversão de dependência
    /// </summary>
    public interface IAudioService
    {
        // Sound Effects
        AudioSource PlaySoundEffect(string filePath, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false);
        AudioSource PlaySoundEffect(AudioClip clip, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false);
        void StopSoundEffect(string soundName);
        void StopSoundEffect(AudioClip clip);
        
        // Voice
        AudioSource PlayVoice(string filePath, float volume = 1, float pitch = 1, bool loop = false);
        AudioSource PlayVoice(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false);
        
        // Music Tracks
        AudioTrack PlayTrack(string filePath, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f);
        AudioTrack PlayTrack(AudioClip clip, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f, string filePath = "");
        void StopTrack(int channel);
        void StopTrack(string trackName);
        
        // Channel Management
        AudioChannel GetChannel(int channelNumber);
        AudioChannel GetOrCreateChannel(int channelNumber);
        
        // Events
        event System.Action<AudioClip> OnSoundEffectPlayed;
        event System.Action<AudioTrack> OnTrackStarted;
        event System.Action<string> OnTrackStopped;
        event System.Action<System.Exception> OnAudioError;
    }
}
