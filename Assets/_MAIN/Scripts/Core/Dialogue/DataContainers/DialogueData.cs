using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _MAIN.Scripts.Enums;

namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    public class DialogueData 
    {
        public List<DialogueSegment> LineSegments;
        private const string SegmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";
        
        public DialogueData(string rawDialogue)
        {
            LineSegments = RipSegments(rawDialogue);
        }

        private List<DialogueSegment> RipSegments(string rawDialogue)
        {
            var segments = new List<DialogueSegment>();
            var matches = Regex.Matches(rawDialogue, SegmentIdentifierPattern);

            var lastIndex = 0;
            var segment = new DialogueSegment();
            segment.Dialogue = (matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index));
            segment.StartSignal = EStartSignal.NONE;
            segment.SignalDelay = 0;
            segments.Add(segment);

            if (matches.Count == 0)
                return segments;
            
            lastIndex = matches[0].Index;

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                segment = new DialogueSegment();

                //Get StartSignal
                var signalMatch = match.Value;
                signalMatch = signalMatch.Substring(1, match.Length - 2);
                var signalSplit = signalMatch.Split(' ');

                segment.StartSignal = (EStartSignal)Enum.Parse(typeof(EStartSignal), signalSplit[0].ToUpper());
                
                //Get Delay
                if (signalSplit.Length > 1)
                    float.TryParse(signalSplit[1], out segment.SignalDelay);
                
                //GetDialogue
                var nextIndex = i + 1 < matches.Count ? matches[i + 1].Index : rawDialogue.Length;
                segment.Dialogue =
                rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length));
                lastIndex = nextIndex;
                
                segments.Add(segment);
            }

            return segments;
        }
    }
    
    public struct DialogueSegment
    {
        public string Dialogue;
        public EStartSignal StartSignal;
        public float SignalDelay;
        public bool AppendText => StartSignal is EStartSignal.A or EStartSignal.WA;

    }
}
