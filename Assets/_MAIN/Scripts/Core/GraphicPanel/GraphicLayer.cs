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

        public GraphicObject CurrentGraphic;
        public List<GraphicObject> OldGraphics = new();

        public Coroutine SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null, bool immediate = false)
        {
            Texture tex = Resources.Load<Texture2D>(filePath);

            if (tex == null)
            {
                Debug.LogError($"Could not load graphic texture from path '{filePath}.' Please ensure it exists within Resources!");
                return null;
            }

            return SetTexture(tex, transitionSpeed, blendingTexture, filePath);
        }

        public Coroutine SetTexture(Texture tex, float transitionSpeed = 1f, Texture blendingTexture = null, string filepath = "", bool immediate = false)
        {
            return CreateGraphic(tex, transitionSpeed, filepath, blendingTexture: blendingTexture, immediate: immediate);
        }

        public Coroutine SetVideo(string filePath, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null, bool immediate = false)
        {
            VideoClip clip = Resources.Load<VideoClip>(filePath);

            if (clip == null)
            {
                Debug.LogError($"Could not load graphic video from path '{filePath}.' Please ensure it exists within Resources!");
                return null;
            }

            return SetVideo(clip, transitionSpeed, useAudio, blendingTexture, filePath);
        }

        public Coroutine SetVideo(VideoClip video, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null, string filepath = "", bool immediate = false)
        {
            return CreateGraphic(video, transitionSpeed, filepath, useAudio, blendingTexture, immediate);
        }

        private Coroutine CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true, Texture blendingTexture = null, bool immediate = false)
        {
            GraphicObject newGraphic = null;

            if (graphicData is Texture)
                newGraphic = new GraphicObject(this, filePath, graphicData as Texture, immediate);
            else if (graphicData is VideoClip)
                newGraphic = new GraphicObject(this, filePath, graphicData as VideoClip, useAudioForVideo, immediate);

            if (CurrentGraphic != null && !OldGraphics.Contains(CurrentGraphic))
                OldGraphics.Add(CurrentGraphic);

            CurrentGraphic = newGraphic;

            if (!immediate)
                return CurrentGraphic.FadeIn(transitionSpeed, blendingTexture);
            
            DestroyOldGraphics();
            return null;
        }

        public void DestroyOldGraphics()
        {
            foreach (var g in OldGraphics)
                    Object.Destroy(g.Renderer.gameObject);

            OldGraphics.Clear();
        }

        public void Clear(float transitionSpeed = 1, Texture blendTexture = null, bool immediate = false)
        {
            if (CurrentGraphic != null)
            {
                if (!immediate)
                    CurrentGraphic.FadeOut(transitionSpeed, blendTexture);
                else
                    CurrentGraphic.Destroy();
            }

            foreach (var g in OldGraphics)
            {
                if (!immediate)
                    g.FadeOut(transitionSpeed, blendTexture);
                else
                    g.Destroy();
            }
        }
    }
}
