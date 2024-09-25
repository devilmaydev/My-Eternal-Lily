using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace _MAIN.Scripts.Core.GraphicPanel
{
    public class GraphicLayer 
    {
        public const string LayerObjectNameFormat = "Layer: {0}";
        public int LayerDepth = 0;
        public Transform Panel;

        public GraphicObject CurrentGraphic = null;
        private List<GraphicObject> _oldGraphics = new();

        public void SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null)
        {
            Texture tex = Resources.Load<Texture2D>(filePath);

            if (tex == null)
            {
                Debug.LogError($"Could not load graphic texture from path '{filePath}.' Please ensure it exists within Resources!");
                return;
            }

            SetTexture(tex, transitionSpeed, blendingTexture, filePath);
        }

        public void SetTexture(Texture tex, float transitionSpeed = 1f, Texture blendingTexture = null, string filepath = "")
        {
            CreateGraphic(tex, transitionSpeed, filepath, blendingTexture: blendingTexture);
        }

        public void SetVideo(string filePath, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null)
        {
            VideoClip clip = Resources.Load<VideoClip>(filePath);

            if (clip == null)
            {
                Debug.LogError($"Could not load graphic video from path '{filePath}.' Please ensure it exists within Resources!");
                return;
            }

            SetVideo(clip, transitionSpeed, useAudio, blendingTexture, filePath);
        }

        public void SetVideo(VideoClip video, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null, string filepath = "")
        {
            CreateGraphic(video, transitionSpeed, filepath, useAudio, blendingTexture);
        }

        private void CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true, Texture blendingTexture = null)
        {
            GraphicObject newGraphic = null;

            if (graphicData is Texture)
                newGraphic = new GraphicObject(this, filePath, graphicData as Texture);
            else if (graphicData is VideoClip)
                newGraphic = new GraphicObject(this, filePath, graphicData as VideoClip, useAudioForVideo);

            if (CurrentGraphic != null && !_oldGraphics.Contains(CurrentGraphic))
                _oldGraphics.Add(CurrentGraphic);

            CurrentGraphic = newGraphic;

            CurrentGraphic.FadeIn(transitionSpeed, blendingTexture);
        }

        public void DestroyOldGraphics()
        {
            foreach (var g in _oldGraphics)
                Object.Destroy(g.Renderer.gameObject);

            _oldGraphics.Clear();
        }

        public void Clear()
        {
            if (CurrentGraphic != null)
                CurrentGraphic.FadeOut();

            foreach (var g in _oldGraphics)
                g.FadeOut();
        }
    }
}
