using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace _MAIN.Scripts.Core.Audio
{
    public class AudioChannel
    {
        private const string TrackContainerNameFormat = "Channel - [{0}]";
        public int ChannelIndex { get; private set; }

        public Transform TrackContainer { get; }

        public AudioTrack ActiveTrack { get; private set; }
        private List<AudioTrack> _tracks = new();

        private bool IsLevelingVolume => _coVolumeLeveling != null;
        private Coroutine _coVolumeLeveling;

        public AudioChannel(int channel)
        {
            ChannelIndex = channel;

            TrackContainer = new GameObject(string.Format(TrackContainerNameFormat, channel)).transform;
            TrackContainer.SetParent(AudioManager.Instance.transform);
        }

        public AudioTrack PlayTrack(AudioClip clip, bool loop, float startingVolume, float volumeCap, float pitch, string filePath)
        {
            if (TryGetTrack(clip.name, out AudioTrack existingTrack))
            {
                if (!existingTrack.IsPlaying)
                    existingTrack.Play();

                SetAsActiveTrack(existingTrack);

                return existingTrack;
            }

            var track = new AudioTrack(clip, loop, startingVolume, volumeCap, pitch, this, AudioManager.Instance.musicMixer);
            track.Play();

            SetAsActiveTrack(track);

            return track;
        }

        public bool TryGetTrack(string trackName, out AudioTrack value)
        {
            trackName = trackName.ToLower();

            foreach (var track in _tracks)
            {
                if (track.Name.ToLower() == trackName)
                {
                    value = track;
                    return true;
                }
            }

            value = null;
            return false;
        }

        private void SetAsActiveTrack(AudioTrack track)
        {
            if (!_tracks.Contains(track))
                _tracks.Add(track);

            ActiveTrack = track;

            TryStartVolumeLeveling();
        }

        private void TryStartVolumeLeveling()
        {
            if (!IsLevelingVolume)
                _coVolumeLeveling = AudioManager.Instance.StartCoroutine(VolumeLeveling());
        }

        private IEnumerator VolumeLeveling()
        {
            while ((ActiveTrack != null && (_tracks.Count > 1 || !Mathf.Approximately(ActiveTrack.Volume, ActiveTrack.VolumeCap))) || (ActiveTrack == null && _tracks.Count > 0))
            {
                for (int i = _tracks.Count - 1; i >= 0; i--)
                {
                    AudioTrack track = _tracks[i];

                    float targetVol = ActiveTrack == track ? track.VolumeCap : 0;

                    if (track == ActiveTrack && Mathf.Approximately(track.Volume, targetVol))
                        continue;

                    track.Volume = Mathf.MoveTowards(track.Volume, targetVol, AudioManager.TrackTransitionSpeed * Time.deltaTime);

                    if (track != ActiveTrack && track.Volume == 0)
                    {
                        DestroyTrack(track);
                    }
                }
                yield return null;
            }

            _coVolumeLeveling = null;
        }

        private void DestroyTrack(AudioTrack track)
        {
            if (_tracks.Contains(track))
                _tracks.Remove(track);

            Object.Destroy(track.Root);
        }

        public void StopTrack()
        {
            if (ActiveTrack == null)
                return;

            ActiveTrack = null;
            TryStartVolumeLeveling();
        }
    }
}