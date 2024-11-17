using System.Collections;
using _MAIN.Scripts.Core.Audio;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Characters.Types;
using _MAIN.Scripts.Core.GraphicPanel;
using UnityEngine;

namespace _TESTING.Scripts
{
    public class AudioTesting : MonoBehaviour
    {
        void Start() => StartCoroutine(Running3());

        Character CreateCharacter(string name) => CharacterManager.Instance.CreateCharacter(name);

        IEnumerator Running3()
        {
            yield return new WaitForSeconds(1);

            var Alex = CreateCharacter("Alex") as CharacterSprite;
            Alex.Show();
            
            GraphicPanelManager.Instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/EngineRoom");
            AudioManager.Instance.PlayTrack("Audio/Music/Upbeat", volumeCap: 0.5f);

            yield return null;
        }
    }
}
