using System.Collections;
using UnityEngine;

public class CommandTesting : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Running());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CommandManager.Instance.Execute("MoveCharacter", "left");
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
            CommandManager.Instance.Execute("MoveCharacter", "right");
        
    }

    private IEnumerator Running()
    {
        yield return CommandManager.Instance.Execute("Print");
        yield return CommandManager.Instance.Execute("PrintOneParameter", "Opa moshi moshi");
        yield return CommandManager.Instance.Execute("PrintLines", "Linha 1","Linha 2","Linha 3");
        
        yield return CommandManager.Instance.Execute("Lambda");
        yield return CommandManager.Instance.Execute("LambdaOneParameter", "Opa boa tarde");
        yield return CommandManager.Instance.Execute("LambdaMultiParameter", "Linha 1","Linha 2","Linha 3");
        
        yield return CommandManager.Instance.Execute("SimpleProcess");
        yield return CommandManager.Instance.Execute("LineProcess", "3");
        yield return CommandManager.Instance.Execute("LinesProcess", "Process 1","Process 2","Process 3");
    }
}
