using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _MAIN.Scripts.Core.Dialogue.Managers
{
    public class TagManager
    {
        private readonly Dictionary<string, Func<string>> tags = new();
        private readonly Regex _tagRegex = new("<\\w+>");

        public TagManager()
        {
            InitializeTags();
        }

        private void InitializeTags()
        {
            tags["<mainChar>"] = () => "Avira";
            tags["<time>"] = () => DateTime.Now.ToString("hh:mm tt");
            tags["<playerLevel>"] = () => "15";
            tags["<tempVal1>"] = () => "42";
        }

        public string Inject(string text)
        {
            if (_tagRegex.IsMatch(text))
            {
                foreach (Match match in _tagRegex.Matches(text))
                {
                    if (tags.TryGetValue(match.Value, out var tagValueRequest))
                    {
                        text = text.Replace(match.Value, tagValueRequest());
                    }
                }
            }

            return text;
        }
    }
}
