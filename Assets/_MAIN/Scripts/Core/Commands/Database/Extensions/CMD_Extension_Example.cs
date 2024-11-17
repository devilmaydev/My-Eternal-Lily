using System;
using System.Collections;
using _MAIN.Scripts.Core.Commands.Database;
using UnityEngine;

public class CMD_Extension_Example : CmdDatabaseExtension
{
    public new static void Extend(CommandsDatabase database)
    {
        //Add Action with no parameters
        database.AddCommand("Print", new Action(PrintDebugMessage));
        database.AddCommand("PrintOneParameter", new Action<string>(PrintUserMessage));
        database.AddCommand("PrintLines", new Action<string[]>(PrintLines));
        
        //Add lambda expression with no parameters
        database.AddCommand("Lambda", new Action(() => { Debug.Log("Lambda command"); }));
        database.AddCommand("LambdaOneParameter", new Action<string>(arg => { Debug.Log($"Lambda command with arg: {arg}"); }));
        database.AddCommand("LambdaMultiParameter", new Action<string[]>(args => { Debug.Log(string.Join(",", args)); }));
        
        //Add coroutine with no parameters
        database.AddCommand("SimpleProcess", new Func<IEnumerator>(SimpleProcess));
        database.AddCommand("LineProcess", new Func<string, IEnumerator>(LineProcess));
        database.AddCommand("LinesProcess", new Func<string[], IEnumerator>(LinesProcess));
        
        //Test
        //database.AddCommand("MoveCharacter", new Func<string, IEnumerator>(MoveCharacter));
    }

    private static void PrintDebugMessage()
    {
        Debug.Log("Printing Debug Message!");
    }

    private static void PrintUserMessage(string message)
    {
        Debug.Log($"Printing User Message: {message}");
    }
    
    private static void PrintLines(string[] lines)
    {
        var i = 1;
        foreach (var line in lines)
        {
            Debug.Log($"{i++} {line}");
        }
    }

    private static IEnumerator SimpleProcess()
    {
        for (int i = 0; i <= 5; i++)
        {
            Debug.Log($"Process is running... [{i}]");
            yield return new WaitForSeconds(1);
        }
    }
    
    private static IEnumerator LineProcess(string data)
    {
        if (int.TryParse(data, out int num))
        {
            for (int i = 0; i <= num; i++)
            {
                Debug.Log($"ProcessLine is running... [{i}]");
                yield return new WaitForSeconds(1);
            }
            
        }
    }
    
    private static IEnumerator LinesProcess(string[] data)
    {
        foreach (var line in data)
        {
            Debug.Log($"Process message: [{line}]");
            yield return new WaitForSeconds(0.5F);
        }
    }

    // private static IEnumerator MoveCharacter(string direction)
    // {
    //     var isLeft = direction.ToLower() == "left";
    //
    //     var character = GameObject.Find("Image").transform;
    //     float moveSpeed = 15;
    //
    //     float targetX = isLeft ? -8 : 8;
    //
    //     float currentX = character.position.x;
    //
    //     while (Mathf.Abs(targetX - currentX) > 0.1f)
    //     {
    //         currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
    //         character.position = new Vector3(currentX, character.position.y, character.position.z);
    //         yield return null;
    //     }
    //     
    //     
    // }

}
