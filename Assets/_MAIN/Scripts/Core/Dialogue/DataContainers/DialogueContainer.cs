using UnityEngine;
using TMPro;

namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject Root;
        public SpeakerContainer SpeakerText;
        public TextMeshProUGUI DialogueText;
        
    }
}
