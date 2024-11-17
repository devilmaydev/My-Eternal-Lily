using System.Collections;
using _MAIN.Scripts.Core.Dialogue.DataContainers;

namespace _MAIN.Scripts.Core.Logical_LInes
{
    public interface ILogicalLine
    {
        string Keyword { get; }
        bool Matches(DialogueLine line);
        IEnumerator Execute(DialogueLine line);
    }
}
