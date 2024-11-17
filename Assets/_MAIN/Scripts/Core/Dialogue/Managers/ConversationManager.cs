using System.Collections;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Dialogue.DataContainers;
using _MAIN.Scripts.Core.Logical_LInes;
using _MAIN.Scripts.Enums;
using _MAIN.Scripts.Extensions;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue.Managers
{
    public class ConversationManager
    {
        private DialogueSystem _dialogueSystem = DialogueSystem.Instance;
        public TextArchitect TextArchitect;
        private TagManager _tagManager;
        private LogicalLineManager _logicalLineManager;
        
        private Coroutine _process;
        public bool IsRunning => _process != null;

        private bool _userPrompt;
        
        public Conversation Conversation => _conversationQueue.IsEmpty() ? null : _conversationQueue.Top;
        public int ConversationProgress => _conversationQueue.IsEmpty() ? -1 : _conversationQueue.Top.GetProgress();
        private ConversationQueue _conversationQueue;

        public ConversationManager(TextArchitect textArchitect)
        {
            TextArchitect = textArchitect;
            _dialogueSystem.OnUserPromptNextEvent += OnUserPromptNext;
            
            _tagManager = new TagManager();
            _logicalLineManager = new LogicalLineManager();

            _conversationQueue = new ConversationQueue();
        }

        private void OnUserPromptNext() => _userPrompt = true;
        public void Enqueue(Conversation conversation) => _conversationQueue.Enqueue(conversation);
        public void EnqueuePriority(Conversation conversation) => _conversationQueue.EnqueuePriority(conversation);
        
        public Coroutine StartConversation(Conversation conversation)
        {
            StopConversation();
            
            Enqueue(conversation);

            _process = _dialogueSystem.StartCoroutine(RunningConversation());

            return _process;
        }
        
        public void StopConversation()
        {
            if (!IsRunning)
                return;
            
            _dialogueSystem.StopCoroutine(_process);
            _process = null;
        }
        
        private IEnumerator RunningConversation()
        {
            while (!_conversationQueue.IsEmpty())
            {
                var currentConversation = Conversation;

                if (currentConversation.HasReachedEnd())
                {
                    _conversationQueue.Dequeue();
                    continue;
                }

                var rawLine = currentConversation.CurrentLine();
                
                //Don't show any blank lines or try to run any logic on them.
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    TryAdvanceConversation(currentConversation);
                    continue;
                }

                var line = DialogueParser.Parse(rawLine);

                if (_logicalLineManager.TryGetLogic(line, out Coroutine logic))
                {
                    yield return logic;
                }
                else
                {
                    if (line.HasDialogue)
                        yield return RunDialogueLine(line);

                    if (line.HasCommands)
                        yield return RunCommandsLine(line);

                    if (line.HasDialogue)
                    {
                        yield return WaitUserInput();
                        CommandManager.Instance.StopAllProcesses();
                    }
                }
                
                TryAdvanceConversation(currentConversation);
            }

            _process = null;
        }
        
        private void TryAdvanceConversation(Conversation conversation)
        {
            conversation.IncrementProgress();

            if (conversation != _conversationQueue.Top)
                return;

            if (conversation.HasReachedEnd())
                _conversationQueue.Dequeue();
        }
        
        private void HandleSpeakerLogic(SpeakerData speakerData)
        {
            bool characterMustBeCreated = speakerData.MakeCharacterEnter || speakerData.IsCastingPosition || speakerData.IsCastingExpressions;

            var character = CharacterManager.Instance.GetCharacter(speakerData.Name, createIfDoesNotExist: characterMustBeCreated);

            if (speakerData.MakeCharacterEnter && !character.IsVisible && !character.IsRevealing)
                character.Show();

            _dialogueSystem.ShowSpeakerName(speakerData.DisplayName);
            
            DialogueSystem.Instance.ApplySpeakerDataToDialogueContainer(speakerData.Name);

            if (speakerData.IsCastingPosition)
                character.MoveToPosition(speakerData.CastPosition);

            if (!speakerData.IsCastingExpressions) 
                return;
            
            foreach (var ce in speakerData.CastExpressions)
                character.OnReceiveCastingExpression(ce.layer, ce.expression);
        }

        private IEnumerator RunDialogueLine(DialogueLine line)
        {
            if (line.HasSpeaker)
                HandleSpeakerLogic(line.SpeakerData);

            if (!_dialogueSystem.dialogueContainer.IsVisible)
                _dialogueSystem.dialogueContainer.Show();
            
            yield return BuildLineSegments(line.DialogueData);
        }

        private IEnumerator RunCommandsLine(DialogueLine line)
        {
            var commands = line.CommandsData.Commands;

            foreach (var command in commands)
            {
                if (command.WaitForCompletion || command.Name == "wait")
                {
                    CoroutineWrapper cw = CommandManager.Instance.Execute(command.Name, command.Arguments);
                    while (!cw.IsDone)
                    {
                        if (_userPrompt)
                        {
                            CommandManager.Instance.StopCurrentProcess();
                            _userPrompt = false;
                        }
                        yield return null;
                    }
                }
                else
                    CommandManager.Instance.Execute(command.Name, command.Arguments);
            }
            
            yield return null;
        }

        private IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            dialogue = _tagManager.Inject(dialogue);
            
            if (append)
                TextArchitect.Append(dialogue);
            else
                TextArchitect.Build(dialogue);

            while (TextArchitect.IsBuilding)
            {
                if (_userPrompt)
                {
                    if (!TextArchitect.HurryUp)
                        TextArchitect.HurryUp = true;
                    else
                        TextArchitect.ForceComplete();
                    
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
                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);
                yield return BuildDialogue(segment.Dialogue, segment.AppendText);
            }
        }

        public bool IsWaitingOnAutoTimer { get; private set; } = false;

        private IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DialogueSegment dialogueSegment)
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
                    IsWaitingOnAutoTimer = true;
                    yield return new WaitForSeconds(dialogueSegment.SignalDelay);
                    IsWaitingOnAutoTimer = false;
                    break;
                default:
                    break;
            }
        }

        private IEnumerator WaitUserInput()
        {
            _dialogueSystem.dialogueContinuePrompt.Show();
            
            while (!_userPrompt)
                yield return null;

            _dialogueSystem.dialogueContinuePrompt.Hide();
            
            _userPrompt = false;
        }
        
    }
}
