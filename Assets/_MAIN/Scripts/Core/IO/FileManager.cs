using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _MAIN.Scripts.Core.IO
{
    public class FileManager
    {
        public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
        {
            if (!filePath.StartsWith('/'))
                filePath = FilePaths.Root + filePath;

            List<string> lines = new();
            try
            {
                using var sr = new StreamReader(filePath);
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }
            }
            catch (FileNotFoundException ex)
            {
                Debug.LogError($"File not found: '{ex.FileName}'");
            }
        
            return lines;
        }
    
        public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
        {
            var asset = Resources.Load<TextAsset>(filePath);
            if (asset == null)
            {
                Debug.LogError($"Asset not found: '{filePath}'");
                return null;
            }

            return ReadTextAsset(asset, includeBlankLines);
        }

        public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
        {
            List<string> lines = new();
            using var sr = new StringReader(asset.text);

            while (sr.Peek() > -1)
            {
                var line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }

            return lines;
        }

    }
}
