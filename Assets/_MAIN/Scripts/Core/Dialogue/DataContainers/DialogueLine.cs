using System;

namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    public class DialogueLine
    {
        public string Speaker;
        public string Dialogue;
        public string Commands;

        public bool HasSpeaker => Speaker != string.Empty;
        public bool HasDialogue => Dialogue != string.Empty;
        public bool HasCommands => Commands != string.Empty;

        public DialogueLine(string speaker, string dialogue, string commands)
        {
            Speaker = speaker;
            Dialogue = dialogue;
            Commands = commands;
        }
    }
}
