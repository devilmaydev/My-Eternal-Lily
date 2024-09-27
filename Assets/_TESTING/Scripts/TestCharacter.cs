using System.Collections;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Characters.Types;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    void Start() => StartCoroutine(Test());
    
    IEnumerator Test()
    {
        var Spada = CharacterManager.Instance.CreateCharacter("Conor Spada") as CharacterSprite;

        yield return new WaitForSeconds(1);
        yield return Spada.UnHighlight();
        
        yield return new WaitForSeconds(1);
        yield return Spada.TransitionColor(Color.blue);
        
        yield return new WaitForSeconds(1);
        yield return Spada.TransitionColor(Color.white);
        
        yield return new WaitForSeconds(1);
        yield return Spada.Highlight();
    }
}
