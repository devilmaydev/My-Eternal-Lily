using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            PromptAdvance();
    }

    public void PromptAdvance()
    {
        DialogueSystem.Instance.OnUserPromptNext();
    }
}
