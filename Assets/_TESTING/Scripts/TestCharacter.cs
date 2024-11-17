using System.Collections;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Characters.Types;
using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.Feature_Panels.ChoicePanel;
using _MAIN.Scripts.Core.GraphicPanel;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    //void Start() => StartCoroutine(RunningLayers());
    
    private ChoicePanel panel;

    private void Start()
    {
        sla();
    }

    private void sla()
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
    
    IEnumerator Test()
    {
        StartCoroutine(RunningLayers());

        return null;
    }

    IEnumerator RunningLayers()
    {
        var panel = GraphicPanelManager.Instance.GetPanel("Background");
        var layer0 = panel.GetLayer(0, true);
        var layer1 = panel.GetLayer(1, true);
        
        layer0.SetVideo("Graphics/BG Videos/Nebula");
        layer1.SetTexture("Graphics/BG Images/SpaceshipInterior");
        
        yield return null;
    }
}
