using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.IO;
using UnityEngine;

public class TestFile : MonoBehaviour
{
    [SerializeField] public string arquivo;
    void Start()
    {
        StartConversation();
    }
    void StartConversation()
    {
        var lines = FileManager.ReadTextAsset(FilePaths.ResourcesDialogueFiles + arquivo);
        DialogueSystem.Instance.Say(lines);
    }

}
