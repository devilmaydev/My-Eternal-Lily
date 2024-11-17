using System;
using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Feature_Panels.ChoicePanel;
using UnityEngine;

public class ChoiceTest : MonoBehaviour
{
    private ChoicePanel panel;

    private void Start()
    {
        panel = ChoicePanel.Instance;

        string[] choices =
        {
            "Choice 1",
            "Choice 2",
            "Choice 3",
            "Choice 4",
            "Choice 5",
            "Choice 6",
        };
        
        panel.Show("Choose something:", choices);
    }
}
