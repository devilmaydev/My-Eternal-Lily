using UnityEngine;
using UnityEngine.Audio;

namespace _MAIN.Scripts.Core.Audio
{
    public class AudioTrack
    {
        private const string TrackNameFormat = "Track - [{0}]";
        public string Name { get; }

        public GameObject Root => _source.gameObject;

        private readonly AudioChannel _channel;
        private readonly AudioSource _source;
        public bool Loop => _source.loop;
        public float VolumeCap { get; private set; }

        public bool IsPlaying => _source.isPlaying;

        public float Volume { 
            get => _source.volume;
            set => _source.volume = value;
        }

        public AudioTrack(AudioClip clip, bool loop, float startingVolume, float volumeCap, float pitch, AudioChannel channel, AudioMixerGroup mixer)
        {
            Name = clip.name;   
            _channel = channel;
            VolumeCap = volumeCap;

            _source = CreateSource();
            _source.clip = clip;
            _source.loop = loop;
            _source.volume = startingVolume;
            _source.pitch = pitch;

            _source.outputAudioMixerGroup = mixer;
        }
        
        public void Play() => _source.Play();
        
        public void Stop() => _source.Stop();

        private AudioSource CreateSource()
        {
            var go = new GameObject(string.Format(TrackNameFormat, Name));
            go.transform.SetParent(_channel.TrackContainer);
            var source = go.AddComponent<AudioSource>();

            return source;
        }
    }
}