namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    public class DialogueLine
    {
        public string RawData { get; private set; } = string.Empty;
        
        public SpeakerData SpeakerData;
        public DialogueData DialogueData;
        public CommandsData CommandsData;

        public bool HasSpeaker => SpeakerData != null;
        public bool HasDialogue => DialogueData != null;
        public bool HasCommands => CommandsData != null;

        public DialogueLine(string rawLine, string speaker, string dialogue, string commands)
        {
            RawData = rawLine;
            SpeakerData = string.IsNullOrWhiteSpace(speaker) ? null : new SpeakerData(speaker);
            DialogueData = string.IsNullOrWhiteSpace(dialogue) ? null : new DialogueData(dialogue);
            CommandsData = string.IsNullOrWhiteSpace(commands) ? null : new CommandsData(commands);
        }
    }
}
