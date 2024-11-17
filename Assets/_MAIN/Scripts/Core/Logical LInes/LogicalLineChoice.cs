using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.Dialogue.DataContainers;
using _MAIN.Scripts.Core.Dialogue.Managers;
using _MAIN.Scripts.Core.Feature_Panels.ChoicePanel;
using UnityEngine;

namespace _MAIN.Scripts.Core.Logical_LInes
{
    public class LogicalLineChoice : ILogicalLine
    {
        public string Keyword => "choice";
        private const char EncapsulationStart = '{';
        private const char EncapsulationEnd = '}';
        private const char ChoiceIdentifier = '-';

        public IEnumerator Execute(DialogueLine line)
        {
            RawChoiceData data = RipChoiceData();
            List<Choice> choices = GetChoicesFromData(data);

            string title = line.DialogueData.RawData;
            ChoicePanel panel = ChoicePanel.Instance;
            string[] choiceTitles = choices.Select(c => c.Title).ToArray();

            panel.Show(title, choiceTitles);

            while (panel.IsWaitingOnUserChoice)
                yield return null;

            Choice selectedChoice = choices[panel.LastDecision.AnswerIndex];

            Conversation newConversation = new Conversation(selectedChoice.ResultLines);
            DialogueSystem.Instance.ConversationManager.Conversation.SetProgress(data.EndingIndex);
            DialogueSystem.Instance.ConversationManager.EnqueuePriority(newConversation);
        }

        public bool Matches(DialogueLine line) => line.HasSpeaker && line.SpeakerData.Name.ToLower() == Keyword;

        private RawChoiceData RipChoiceData()
        {
            Conversation currentConversation = DialogueSystem.Instance.ConversationManager.Conversation;
            int currentProgress = DialogueSystem.Instance.ConversationManager.ConversationProgress;
            int encapsulationDepth = 0;
            RawChoiceData data = new RawChoiceData { Lines = new List<string>(), EndingIndex = 0 };

            for (int i = currentProgress; i < currentConversation.Count; i++)
            {
                string line = currentConversation.GetLines()[i];
                data.Lines.Add(line);

                if (IsEncapsulationStart(line))
                {
                    encapsulationDepth++;
                    continue;
                }

                if (IsEncapsulationEnd(line))
                {
                    encapsulationDepth--;
                    if (encapsulationDepth == 0)
                    {
                        data.EndingIndex = i;
                        break;
                    }
                }
            }

            return data;
        }

        private List<Choice> GetChoicesFromData(RawChoiceData data)
        {
            List<Choice> choices = new List<Choice>();
            int encapsulationDepth = 0;
            bool isFirstChoice = true;

            Choice choice = new Choice
            {
                Title = string.Empty,
                ResultLines = new List<string>(),
            };

            foreach (var line in data.Lines.Skip(1))
            {
                Debug.Log($"'{line}' at encap[{encapsulationDepth}] is choice={IsChoiceStart(line)}");
                if (IsChoiceStart(line) && encapsulationDepth == 1)
                {
                    if (!isFirstChoice)
                    {
                        choices.Add(choice);
                        choice = new Choice
                        {
                            Title = string.Empty,
                            ResultLines = new List<string>(),
                        };
                    }

                    choice.Title = line.Trim().Substring(1);
                    isFirstChoice = false;
                    continue;
                }

                AddLineToResults(line, ref choice, ref encapsulationDepth);
            }

            if (!choices.Contains(choice))
                choices.Add(choice);

            return choices;
        }

        private void AddLineToResults(string line, ref Choice choice, ref int encapsulationDepth)
        {
            line.Trim();

            if (IsEncapsulationStart(line))
            {
                if (encapsulationDepth > 0)
                    choice.ResultLines.Add(line);
                encapsulationDepth++;
                return;
            }

            if (IsEncapsulationEnd(line))
            {
                encapsulationDepth--;

                if (encapsulationDepth > 0)
                    choice.ResultLines.Add(line);

                return;
            }

            choice.ResultLines.Add(line);
        }

        private bool IsEncapsulationStart(string line) => line.Trim().StartsWith(EncapsulationStart);
        private bool IsEncapsulationEnd(string line) => line.Trim().StartsWith(EncapsulationEnd);
        private bool IsChoiceStart(string line) => line.Trim().StartsWith(ChoiceIdentifier);

        private struct RawChoiceData
        {
            public List<string> Lines;
            public int EndingIndex;
        }

        private struct Choice
        {
            public string Title;
            public List<string> ResultLines;
        }
    }
}
