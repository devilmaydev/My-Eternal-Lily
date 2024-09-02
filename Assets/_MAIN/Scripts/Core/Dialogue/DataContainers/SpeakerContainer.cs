using System;
using TMPro;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue.DataContainers
{
    [Serializable]
    public class SpeakerContainer
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI speakerText;
    
        public void ShowSpeaker(string speaker = "")
        {
            root.SetActive(true);

            if (speaker != string.Empty)
                speakerText.text = speaker;

        }

        public void HideSpeaker()
        {
            root.SetActive(false);
        }
    }
}
