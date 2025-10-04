using UnityEngine;

namespace _MAIN.Scripts.Core.IO
{
    public abstract class FilePaths
    {
        private const string HomeDirectorySymbol = "~/";
        
        public static readonly string Root = $"{Application.dataPath}/GameData/";
        
        private static readonly string ResourcesGraphics = "Graphics/";
        
        public static readonly string ResourcesBgImages = $"{ResourcesGraphics}BG Images/";
        public static readonly string ResourcesBgVideos = $"{ResourcesGraphics}BG Videos/";
        public static readonly string ResourcesBlendTextures = $"{ResourcesGraphics}Transition Effects/";
        
        public static readonly string ResourcesAudio = "Audio/";
        public static readonly string ResourcesSfx = $"{ResourcesAudio}SFX/";
        public static readonly string ResourcesVoices = $"{ResourcesAudio}Voices/";
        public static readonly string ResourcesMusic = $"{ResourcesAudio}Music/";
        public static readonly string ResourcesAmbience = $"{ResourcesAudio}Ambience/";
        
        public static readonly string ResourcesDialogueFiles = "DialogueFiles/";

        public static string GetPathToResource(string defaultPath, string resourceName)
        {
            if (resourceName.StartsWith(HomeDirectorySymbol))
                return resourceName.Substring(HomeDirectorySymbol.Length);

            return defaultPath + resourceName;
        }

    }
}
