namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    public class DialogueLine
    {
        public SpeakerData SpeakerData;
        public DialogueData DialogueData;
        public CommandsData CommandsData;

        public bool HasSpeaker => SpeakerData != null;
        public bool HasDialogue => DialogueData != null;
        public bool HasCommands => CommandsData != null;

        public DialogueLine(string speaker, string dialogue, string commands)
        {
            SpeakerData = string.IsNullOrWhiteSpace(speaker) ? null : new SpeakerData(speaker);
            DialogueData = string.IsNullOrWhiteSpace(dialogue) ? null : new DialogueData(dialogue);
            CommandsData = string.IsNullOrWhiteSpace(commands) ? null : new CommandsData(commands);
        }
    }
}
