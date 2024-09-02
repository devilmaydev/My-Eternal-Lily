using System.Text.RegularExpressions;
using _MAIN.Scripts.Core.Dialogue.DataContainers;

namespace _MAIN.Scripts.Core.Dialogue
{
    public class DialogueParser
    {
        private const string CommandRegexPattern = @"\w*[^\s]\(";
        
        public static DialogueLine Parse(string rawLine)
        {
            var (speaker, dialogue, commands) = RipContent(rawLine);
            
            return new DialogueLine(speaker, dialogue, commands);
        }

        private static (string, string, string) RipContent(string rawLine)
        {
            string speaker= "", dialogue = "", commands = "";

            var dialogueStart = -1;
            var dialogueEnd = -1;
            var isEscaped = false;
            
            for (int i = 0; i < rawLine.Length; i++)
            {
                var currentCharacter = rawLine[i];
                
                if (currentCharacter == '\\')
                    isEscaped = !isEscaped;
                else if (currentCharacter == '"' && !isEscaped)
                {
                    if (dialogueStart == -1)
                        dialogueStart = i;
                    else if (dialogueEnd == -1)
                        dialogueEnd = i;
                }
                else
                    isEscaped = false;
            }

            Regex commandRegex = new(CommandRegexPattern);
            var match = commandRegex.Match(rawLine);
            var commandStart = -1;
            if (match.Success)
            {
                commandStart = match.Index;

                if (dialogueStart == -1 && dialogueEnd == -1)
                    return ("", "", rawLine.Trim());
            }

            if (dialogueStart !=1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"","\"");
                if (commandStart != -1)
                {
                    commands = rawLine.Substring(commandStart).Trim();
                }
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
                commands = rawLine;
            else
                speaker = rawLine;
            
            return (speaker, dialogue, commands);
        }
    }
}
