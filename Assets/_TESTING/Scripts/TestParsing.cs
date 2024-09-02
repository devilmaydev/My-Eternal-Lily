using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.IO;
using UnityEngine;

namespace _TESTING.Scripts
{
    public class TestParsing : MonoBehaviour
    {
        private void Start()
        {
            SendFileToParse();
        }

        void SendFileToParse()
        {
            var lines = FileManager.ReadTextAsset("testFile");

            foreach (var line in lines)
            {
                if (line == string.Empty)
                    continue;
                var dialogueLine = DialogueParser.Parse(line);
            }
        }
    }
}
