using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace _MAIN.Scripts.Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private const string SfxParentName = "SFX";
        private const string SfxNameFormat = "SFX - [{0}]";
        public const float TrackTransitionSpeed = 1f;

        public static AudioManager Instance { get; private set; }

        public Dictionary <int, AudioChannel> Channels = new();

        public AudioMixerGroup musicMixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup voicesMixer;

        private Transform _sfxRoot;

        private void Awake()
        {
            if (Instance == null)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }

            _sfxRoot = new GameObject(SfxParentName).transform;
            _sfxRoot.SetParent(transform);
        }

        public AudioSource PlaySoundEffect(string filePath, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
        {
            AudioClip clip = Resources.Load<AudioClip>(filePath);

            if (clip == null)
            {
                Debug.LogError($"Could not load audio file '{filePath}'. Please make sure this exists in the Resources directory!");
                return null;
            }

            return PlaySoundEffect(clip, mixer, volume, pitch, loop);
        }

        public AudioSource PlaySoundEffect(AudioClip clip, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
        {
            AudioSource effectSource = new GameObject(string.Format(SfxNameFormat, clip.name)).AddComponent<AudioSource>();
            effectSource.transform.SetParent(_sfxRoot);
            effectSource.transform.position = _sfxRoot.position;

            effectSource.clip = clip;

            if (mixer == null)
                mixer = sfxMixer;

            effectSource.outputAudioMixerGroup = mixer;
            effectSource.volume = volume;
            effectSource.spatialBlend = 0;
            effectSource.pitch = pitch;
            effectSource.loop = loop;

            effectSource.Play();

            if (!loop)
                Destroy(effectSource.gameObject, (clip.length / pitch) + 1);

            return effectSource;
        }

        public AudioSource PlayVoice(string filePath, float volume = 1, float pitch = 1, bool loop = false)
            => PlaySoundEffect(filePath, voicesMixer, volume, pitch, loop);

        public AudioSource PlayVoice(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false)
            => PlaySoundEffect(clip, voicesMixer, volume, pitch, loop);

        public void StopSoundEffect(AudioClip clip) => StopSoundEffect(clip.name);

        public void StopSoundEffect(string soundName)
        {
            soundName = soundName.ToLower();

            var sources = _sfxRoot.GetComponentsInChildren<AudioSource>();
            foreach (var source in sources)
            {
                if (source.clip.name.ToLower() != soundName) 
                    continue;
                
                Destroy(source.gameObject);
                return;
            }
        }

        public AudioTrack PlayTrack(string filePath, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f)
        {
            var clip = Resources.Load<AudioClip>(filePath);

            if (clip != null) 
                return PlayTrack(clip, channel, loop, startingVolume, volumeCap, pitch, filePath);
            
            Debug.LogError($"Could not load audio file '{filePath}'. Please make sure this exists in the Resources directory!");
            return null;

        }

        public AudioTrack PlayTrack(AudioClip clip, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f, string filePath = "")
        {
            var audioChannel = TryGetChannel(channel, createIfDoesNotExist: true);
            var track = audioChannel.PlayTrack(clip, loop, startingVolume, volumeCap, pitch, filePath);
            return track;
        }

        public void StopTrack(int channel)
        {
            var audioChannel = TryGetChannel(channel, createIfDoesNotExist: false);

            if (audioChannel == null)
                return;

            audioChannel.StopTrack();
        }

        public void StopTrack(string trackName)
        {
            trackName = trackName.ToLower();

            foreach (var channel in Channels.Values)
            {
                if (channel.ActiveTrack == null || channel.ActiveTrack.Name.ToLower() != trackName) 
                    continue;
                
                channel.StopTrack();
                return;
            }
        }

        public AudioChannel TryGetChannel(int channelNumber, bool createIfDoesNotExist = false)
        {
            if (Channels.TryGetValue(channelNumber, out var channel))
                return channel;

            if (!createIfDoesNotExist) 
                return null;
            
            channel = new AudioChannel(channelNumber);
            Channels.Add(channelNumber, channel);
            return channel;
        }
    }
}