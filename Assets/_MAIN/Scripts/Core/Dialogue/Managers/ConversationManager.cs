using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue.DataContainers;
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
                
            }
        }

        private IEnumerator RunDialogueLine(DialogueLine line)
        {
            if (line.HasSpeaker)
                _dialogueSystem.ShowSpeakerName(line.Speaker);
            
            yield return BuildDialogue(line.Dialogue);
            yield return WaitUserInput();
        }

        private IEnumerator RunCommandsLine(DialogueLine line)
        {
            Debug.Log(line.Commands);
            yield return null;
        }

        private IEnumerator BuildDialogue(string dialogue)
        {
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

        private IEnumerator WaitUserInput()
        {
            while (!_userPrompt)
                yield return null;

            _userPrompt = false;
        }
        
    }
}
