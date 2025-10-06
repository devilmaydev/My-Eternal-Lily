using System;
using UnityEngine;

namespace Core.Systems.Audio.Events
{
    /// <summary>
    /// Eventos do sistema de áudio
    /// Para comunicação entre sistemas
    /// </summary>
    public static class AudioEvents
    {
        // Sound Effects Events
        public static event Action<AudioClip> OnSoundEffectPlayed;
        public static event Action<string> OnSoundEffectStopped;
        
        // Music Track Events
        public static event Action<AudioTrack> OnTrackStarted;
        public static event Action<AudioTrack> OnTrackStopped;
        public static event Action<int> OnChannelStopped;
        
        // Voice Events
        public static event Action<AudioClip> OnVoicePlayed;
        public static event Action<string> OnVoiceStopped;
        
        // Error Events
        public static event Action<Exception> OnAudioError;
        public static event Action<string> OnAudioFileNotFound;
        
        // Volume Events
        public static event Action<float> OnMasterVolumeChanged;
        public static event Action<float> OnMusicVolumeChanged;
        public static event Action<float> OnSFXVolumeChanged;
        public static event Action<float> OnVoiceVolumeChanged;
        
        // Channel Events
        public static event Action<int> OnChannelCreated;
        public static event Action<int> OnChannelDestroyed;
        
        // Internal Methods
        internal static void InvokeSoundEffectPlayed(AudioClip clip)
        {
            OnSoundEffectPlayed?.Invoke(clip);
        }
        
        internal static void InvokeSoundEffectStopped(string soundName)
        {
            OnSoundEffectStopped?.Invoke(soundName);
        }
        
        internal static void InvokeTrackStarted(AudioTrack track)
        {
            OnTrackStarted?.Invoke(track);
        }
        
        internal static void InvokeTrackStopped(AudioTrack track)
        {
            OnTrackStopped?.Invoke(track);
        }
        
        internal static void InvokeChannelStopped(int channel)
        {
            OnChannelStopped?.Invoke(channel);
        }
        
        internal static void InvokeVoicePlayed(AudioClip clip)
        {
            OnVoicePlayed?.Invoke(clip);
        }
        
        internal static void InvokeVoiceStopped(string voiceName)
        {
            OnVoiceStopped?.Invoke(voiceName);
        }
        
        internal static void InvokeAudioError(Exception exception)
        {
            OnAudioError?.Invoke(exception);
        }
        
        internal static void InvokeAudioFileNotFound(string filePath)
        {
            OnAudioFileNotFound?.Invoke(filePath);
        }
        
        internal static void InvokeMasterVolumeChanged(float volume)
        {
            OnMasterVolumeChanged?.Invoke(volume);
        }
        
        internal static void InvokeMusicVolumeChanged(float volume)
        {
            OnMusicVolumeChanged?.Invoke(volume);
        }
        
        internal static void InvokeSFXVolumeChanged(float volume)
        {
            OnSFXVolumeChanged?.Invoke(volume);
        }
        
        internal static void InvokeVoiceVolumeChanged(float volume)
        {
            OnVoiceVolumeChanged?.Invoke(volume);
        }
        
        internal static void InvokeChannelCreated(int channel)
        {
            OnChannelCreated?.Invoke(channel);
        }
        
        internal static void InvokeChannelDestroyed(int channel)
        {
            OnChannelDestroyed?.Invoke(channel);
        }
    }
}
