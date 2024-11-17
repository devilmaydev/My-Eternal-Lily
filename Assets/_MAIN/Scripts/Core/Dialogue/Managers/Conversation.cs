using System.Collections.Generic;

namespace _MAIN.Scripts.Core.Dialogue.Managers
{
    public class Conversation
    {
        private List<string> _lines;
        private int _progress;

        public Conversation(List<string> lines, int progress = 0)
        { 
            _lines = lines;
            _progress = progress;
        }

        public int GetProgress() => _progress;
        public void SetProgress(int value) => _progress = value;
        public void IncrementProgress() => _progress++;
        public int Count => _lines.Count;
        public List<string> GetLines() => _lines;  
        public string CurrentLine() => _lines[_progress];
        public bool HasReachedEnd() => _progress >= _lines.Count;
    }
}
