using UnityEngine;

namespace _MAIN.Scripts.Core.GraphicPanel
{
    public class GraphicPanelManager : MonoBehaviour
    {
        public static GraphicPanelManager Instance { get; private set; }

        public const float DefaultTransitionSpeed = 1f;

        [SerializeField] private GraphicPanel[] _allPanels;

        private void Awake()
        {
            Instance = this;
        }

        public GraphicPanel GetPanel(string panelName)
        {
            panelName = panelName.ToLower();

            foreach (var panel in _allPanels) 
            {
                if (panel.PanelName.ToLower() == panelName)
                    return panel;
            }

            return null;
        }
    }
}

