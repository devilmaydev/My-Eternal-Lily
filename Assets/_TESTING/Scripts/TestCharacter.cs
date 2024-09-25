using System.Collections;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Characters.Types;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TestCharacter : MonoBehaviour
{
    void Start() => StartCoroutine(Test());
    
    IEnumerator Test()
    {
        var Spada = CharacterManager.Instance.CreateCharacter("Conor Spada") as CharacterSprite;
        var Alex = CharacterManager.Instance.CreateCharacter("Alex") as CharacterSprite;
        
        Spada.SetPosition(new Vector2(-0.2f, 0f));
        Alex.SetPosition(new Vector2(1.2f, 0f));
        
        Spada.Show();
        Alex.Show();
        
        yield return Spada.MoveToPosition(new Vector2(0.2f, 0f), 4f);

        Spada.SetSprite(Spada.GetSprite("spada_shock"));
        yield return Spada.Say("Opa!");
        
        yield return Alex.MoveToPosition(new Vector2(0.8f, 0f));
        
        yield return Spada.Say("Vo Fica de canto aqui!");
        Alex.SetSprite(Alex.GetSprite("alex_baffled"));
        yield return Alex.Say("Fica de canto msm fdp!");
    }
}
