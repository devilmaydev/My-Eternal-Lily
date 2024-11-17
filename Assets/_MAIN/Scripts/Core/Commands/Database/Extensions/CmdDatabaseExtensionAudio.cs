using System;
using _MAIN.Scripts.Core.Audio;
using _MAIN.Scripts.Core.IO;
using UnityEngine;

namespace _MAIN.Scripts.Core.Commands.Database.Extensions
{
    public class CmdDatabaseExtensionAudio : CmdDatabaseExtension
    { 
        private static string[] ParamSfx => new[] { "-s", "-sfx" };
        private static string[] ParamVolume => new[] { "-v", "-vol", "-volume" };
        private static string[] ParamPitch => new[] { "-p", "-pitch" };
        private static string[] ParamLoop => new[] { "-l", "-loop" };
        
        private static string[] ParamChannel => new[] { "-c", "-channel" };
        private static string[] ParamImmediate => new[] { "-i", "-immediate" };
        private static string[] ParamStartVolume => new[] { "-sv", "-startvolume" };
        private static string[] ParamSong => new[] { "-s", "-song" };
        private static string[] ParamAmbience => new[] { "-a", "-ambience" };


        public new static void Extend(CommandsDatabase database)
        {
            database.AddCommand("playsfx", new Action<string[]>(PlaySfx));
            database.AddCommand("stopsfx", new Action<string>(StopSfx));
            
            database.AddCommand("playvoice", new Action<string[]>(PlayVoice));
            database.AddCommand("stopvoice", new Action<string>(StopSfx));
            
            database.AddCommand("playsong", new Action<string[]>(PlaySong));
            database.AddCommand("playambience", new Action<string[]>(PlayAmbience));
            
            database.AddCommand("stopsong", new Action<string>(StopSong));
            database.AddCommand("stopambience", new Action<string>(StopAmbience));

        }

        private static void PlaySfx(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamSfx, out string filePath);
            parameters.TryGetValue(ParamVolume, out float volume, defaultValue: 1f);
            parameters.TryGetValue(ParamPitch, out float pitch, defaultValue: 1f);
            parameters.TryGetValue(ParamLoop, out bool loop, defaultValue: false);

            var sound = Resources.Load<AudioClip>(FilePaths.GetPathToResource(FilePaths.ResourcesSfx, filePath));

            if (!sound) return;

            AudioManager.Instance.PlaySoundEffect(sound, volume: volume, pitch: pitch, loop: loop);
        }

        private static void StopSfx(string data)
        {
            AudioManager.Instance.StopSoundEffect(data);
        }

        private static void PlayVoice(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamSfx, out string filePath);
            parameters.TryGetValue(ParamVolume, out float volume, defaultValue: 1f);
            parameters.TryGetValue(ParamPitch, out float pitch, defaultValue: 1f);
            parameters.TryGetValue(ParamLoop, out bool loop, defaultValue: false);

            var sound = Resources.Load<AudioClip>(FilePaths.GetPathToResource(FilePaths.ResourcesVoices, filePath));

            if (!sound) return;

            AudioManager.Instance.PlayVoice(sound, volume: volume, pitch: pitch, loop: loop);
        }

        private static void PlaySong(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamSong, out string filePath);
            filePath = FilePaths.GetPathToResource(FilePaths.ResourcesMusic, filePath);

            parameters.TryGetValue(ParamChannel, out int channel, defaultValue: 1);

            PlayTrack(filePath, channel, parameters);
        }

        private static void PlayAmbience(string[] data)
        {
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamAmbience, out string filePath);
            filePath = FilePaths.GetPathToResource(FilePaths.ResourcesAmbience, filePath);

            parameters.TryGetValue(ParamChannel, out int channel, defaultValue: 1);

            PlayTrack(filePath, channel, parameters);
        }

        private static void PlayTrack(string filePath, int channel, CommandParameters parameters)
        {
            parameters.TryGetValue(ParamVolume, out float volumeCap, defaultValue: 1f);
            parameters.TryGetValue(ParamStartVolume, out float startVolume, defaultValue: 0f);
            parameters.TryGetValue(ParamPitch, out float pitch, defaultValue: 1f);
            parameters.TryGetValue(ParamLoop, out bool loop, defaultValue: true);

            var sound = Resources.Load<AudioClip>(filePath);

            if (!sound) return;

            AudioManager.Instance.PlayTrack(sound, channel, loop,  startVolume, volumeCap, pitch, filePath);
        }

        private static void StopSong(string data)
        {
            if (data == string.Empty)
             StopTrack("1");
            else
                StopTrack(data);
        }
        
        private static void StopAmbience(string data)
        {
            if (data == string.Empty)
                StopTrack("0");
            else
                StopTrack(data);
        }

        private static void StopTrack(string data)
        {
            if (int.TryParse(data, out int channel))
                AudioManager.Instance.StopTrack(channel);
            else
                AudioManager.Instance.StopTrack(data);
        }
    }
}