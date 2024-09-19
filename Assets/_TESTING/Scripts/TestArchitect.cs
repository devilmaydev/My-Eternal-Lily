using _MAIN.Scripts.Core;
using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Enums;
using UnityEngine;

namespace _TESTING.Scripts
{
    public class TestArchitect : MonoBehaviour
    {
        private DialogueSystem _dialogueSystem;
        private TextArchitect _architect;

        private string[] lines = new string[5]
        {
            "This is a random line of dialogue.",
            "I want to say something, come over here.",
            "The world is a crazy place sometimes.",
            "Don't lose hope, things will get better!",
            "It's a bird? It's a plane? No! - It's Super Sheltie!"
        };
        
        void Start()
        {
            _dialogueSystem = DialogueSystem.Instance;
            _architect = new TextArchitect(_dialogueSystem.dialogueContainer.dialogueText)
            {
                BuildMethodChosen = EBuildMethod.Typewriter
            };
        }

        void Update()
        {
            string longLine = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam nec lorem efficitur sapien euismod imperdiet. Proin laoreet posuere vestibulum. Maecenas posuere, urna id posuere pharetra, lacus purus dictum felis, nec malesuada purus quam at risus. Donec vel cursus felis. Donec fringilla molestie diam, a vestibulum nisi faucibus vel. Aenean convallis orci id quam hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Sed feugiat dui turpis, eu viverra metus tristique vitae.";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_architect.IsBuilding)
                {
                    if (!_architect.HurryUp)
                        _architect.HurryUp = true;
                    else
                        _architect.ForceComplete();
                }
                else
                    _architect.Build(longLine);
                //_architect.Build(lines[Random.Range(0, lines.Length)]);
                
            }
            if (Input.GetKeyDown(KeyCode.A))
                _architect.Append(longLine);
                //_architect.Append(lines[Random.Range(0, lines.Length)]);
        }
    }
}
