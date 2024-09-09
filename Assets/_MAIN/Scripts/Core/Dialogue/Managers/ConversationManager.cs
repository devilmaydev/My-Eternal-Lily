using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue.DataContainers;
using _MAIN.Scripts.Enums;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue.Managers
{
    public class ConversationManager
    {
        private DialogueSystem _dialogueSystem = DialogueSystem.Instance;
        private TextArchitect _textArchitect;
        
        private Coroutine _process = null;
        public bool IsRunning => _process != null;

        private bool _userPrompt;

        public ConversationManager(TextArchitect textArchitect)
        {
            _textArchitect = textArchitect;
            _dialogueSystem.OnUserPromptNextEvent += OnUserPromptNext;
        }

        private void OnUserPromptNext()
        {
            _userPrompt = true;
        }
        
        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            _process = _dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }
        
        public void StopConversation()
        {
            if (!IsRunning)
                return;
            
            _dialogueSystem.StopCoroutine(_process);
            _process = null;
        }
        
        private IEnumerator RunningConversation(List<string> conversation)
        {
            foreach (var line in conversation)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                var lineParsed = DialogueParser.Parse(line);
                
                if (lineParsed.HasDialogue)
                    yield return RunDialogueLine(lineParsed);

                if (lineParsed.HasCommands)
                    yield return RunCommandsLine(lineParsed);

                if (lineParsed.HasDialogue)
                    yield return WaitUserInput();
            }
        }

        private IEnumerator RunDialogueLine(DialogueLine line)
        {
            if (line.HasSpeaker)
                _dialogueSystem.ShowSpeakerName(line.SpeakerData.DisplayName);
            
            yield return BuildLineSegments(line.DialogueData);
        }

        private IEnumerator RunCommandsLine(DialogueLine line)
        {
            var commands = line.CommandsData.Commands;

            foreach (var command in commands)
            {
                if (command.WaitForCompletion)
                    yield return CommandManager.Instance.Execute(command.Name, command.Arguments);
                else
                    CommandManager.Instance.Execute(command.Name, command.Arguments);
            }
            
            yield return null;
        }

        private IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            if (append)
                _textArchitect.Append(dialogue);
            else
                _textArchitect.Build(dialogue);

            while (_textArchitect.IsBuilding)
            {
                if (_userPrompt)
                {
                    if (!_textArchitect.HurryUp)
                        _textArchitect.HurryUp = true;
                    else
                        _textArchitect.ForceComplete();
                    
                    _userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator BuildLineSegments(DialogueData dialogueLineData)
        {
            for (int i = 0; i < dialogueLineData.LineSegments.Count; i++)
            {
                var segment = dialogueLineData.LineSegments[i];
                yield return WaitForDialogueSegmentSignalToBeTriggerd(segment);
                yield return BuildDialogue(segment.Dialogue, segment.AppendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggerd(DialogueSegment dialogueSegment)
        {
            switch (dialogueSegment.StartSignal)
            {
                case EStartSignal.NONE:
                    break;
                case EStartSignal.C:
                case EStartSignal.A:
                    yield return WaitUserInput();
                    break;
                case EStartSignal.WC:
                case EStartSignal.WA:
                    yield return new WaitForSeconds(dialogueSegment.SignalDelay);
                    break;
            }
        }

        private IEnumerator WaitUserInput()
        {
            while (!_userPrompt)
                yield return null;

            _userPrompt = false;
        }
        
    }
}
