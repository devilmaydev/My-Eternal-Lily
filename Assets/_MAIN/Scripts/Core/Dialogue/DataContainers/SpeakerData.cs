using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    public class SpeakerData
    {
        public string RawData { get; private set; } = string.Empty;

        public string Name, CastName;
        public Vector2 CastPosition;
        
        public string DisplayName => CastName != string.Empty ? CastName : Name;

        public List<(int layer, string expression)> CastExpressions { get; set; }
        
        public bool IsCastingName => CastName != string.Empty;
        public bool IsCastingPosition = false;
        public bool IsCastingExpressions => CastExpressions.Count > 0;
        public bool MakeCharacterEnter = false;
        
        private const string NameCastID = " as ";
        private const string PositionCastID = " at ";
        private const string ExpressionCastID = " [";
        private const char AxisDelimiter = ':';
        private const char ExpressionLayerJoiner = ',';
        private const char ExpressionLayerDelimiter = ':';
        
        private const string EnterKeyword = "enter ";

        public SpeakerData(string rawSpeaker)
        {
            RawData = rawSpeaker;
            rawSpeaker = ProcessKeywords(rawSpeaker);
            
            string pattern = @" as | at | \[";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            CastName = "";
            CastPosition = Vector2.zero;
            CastExpressions = new List<(int layer, string expression)>();
            
            if (matches.Count == 0)
            {
                Name = rawSpeaker;
                return;
            }

            int index = matches[0].Index;
            Name = rawSpeaker.Substring(0, index);

            for (int i = 0; i < matches.Count(); i++)
            {
                var match = matches[i];
                int startIndex = 0, endIndex = 0;

                if (match.Value == NameCastID)
                {
                    startIndex = match.Index + NameCastID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                    CastName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                else if(match.Value == PositionCastID)
                {
                    IsCastingPosition = true;
                    startIndex = match.Index + PositionCastID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                    string castPosition = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                    string[] axis = castPosition.Split(AxisDelimiter, StringSplitOptions.RemoveEmptyEntries);

                    float.TryParse(axis[0], NumberStyles.Float, CultureInfo.InvariantCulture, out CastPosition.x);

                    if (axis.Length > 1)
                        float.TryParse(axis[1], NumberStyles.Float, CultureInfo.InvariantCulture, out CastPosition.y);

                }
                else if (match.Value == ExpressionCastID)
                {
                    startIndex = match.Index + ExpressionCastID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                    string castExpression = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));

                    CastExpressions = castExpression.Split(ExpressionLayerJoiner)
                        .Select(x =>
                        {
                            var parts = x.Trim().Split(ExpressionLayerDelimiter);
                            return parts.Length == 2 ? (int.Parse(parts[0]), parts[1]) : (0, parts[0]);
                        }).ToList();
                }
            }
            
        }
        
        private string ProcessKeywords(string rawSpeaker)
        {
            if (rawSpeaker.StartsWith(EnterKeyword))
            {
                rawSpeaker = rawSpeaker.Substring(EnterKeyword.Length);
                MakeCharacterEnter = true;
            }

            return rawSpeaker;
        }
        
    }
}
