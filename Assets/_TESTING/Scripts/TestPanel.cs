using System.Collections;
using System.Collections.Generic;
using _MAIN.Scripts.Core.GraphicPanel;
using UnityEngine;

public class TestPanel : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        var panel = GraphicPanelManager.Instance.GetPanel("Background");
        var layer = panel.GetLayer(0, true);

        yield return new WaitForSeconds(1);

        var bTPitchBlack = Resources.Load<Texture>("Graphics/Transition Effects/pitchBlack");
        layer.SetTexture("Graphics/PlayTest/quarto_spada", blendingTexture: bTPitchBlack);
    }
    
}
