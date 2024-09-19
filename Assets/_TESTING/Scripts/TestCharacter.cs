using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Dialogue;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        Character Spada = CharacterManager.Instance.CreateCharacter("Spada");
        //Character Nano = CharacterManager.Instance.CreateCharacter("Nano");

        List<string> lines = new()
        {
            "Oi",
            "td bom?",
            "como vc esta?",
            "eu to bom demais",
            "slc to mt bem msm rapa"
        };

        yield return Spada.Say(lines);
        //
        // lines = new()
        // {
        //     "Oi",
        //     "td bom{c}!!!!"
        // };
        //
        // yield return Nano.Say(lines);
        
        Debug.Log("Done!");
    }
}
