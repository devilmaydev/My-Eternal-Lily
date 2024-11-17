using System.Collections.Generic;
using UnityEngine;

namespace _MAIN.Scripts.Core.GraphicPanel
{
    [System.Serializable]
    public class GraphicPanel
    {
        public string PanelName;
        public GameObject RootPanel;
        private List<GraphicLayer> _layers = new();

        public GraphicLayer GetLayer(int layerDepth, bool createIfDoesNotExist = false)
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                if (_layers[i].LayerDepth == layerDepth)
                    return _layers[i];
            }

            if (createIfDoesNotExist) 
            {       
                return CreateLayer(layerDepth);
            }

            return null;
        }

        private GraphicLayer CreateLayer(int layerDepth)
        {
            GraphicLayer layer = new GraphicLayer();
            GameObject panel = new GameObject(string.Format(GraphicLayer.LayerObjectNameFormat, layerDepth));
            RectTransform rect = panel.AddComponent<RectTransform>();
            panel.AddComponent<CanvasGroup>();
            panel.transform.SetParent(RootPanel.transform, false);

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.one;

            layer.Panel = panel.transform;
            layer.LayerDepth = layerDepth;

            int index = _layers.FindIndex(l => l.LayerDepth > layerDepth);
            if (index == -1)
                _layers.Add(layer);
            else
                _layers.Insert(index, layer);

            for (int i = 0; i < _layers.Count; i++)
                _layers[i].Panel.SetSiblingIndex(_layers[i].LayerDepth);

            return layer;
        }

        public void Clear(float transitionSpeed = 1, Texture blendTexture = null, bool immediate = false)
        {
            foreach(var layer in _layers)
                layer.Clear(transitionSpeed, blendTexture, immediate);
        }
    }
}
