using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace _MAIN.Scripts.Core.GraphicPanel
{
    public class GraphicObject
    {
        GraphicPanelManager PanelManager => GraphicPanelManager.Instance;

        private const string NameFormat = "Graphic - [{0}]";
        private const string MaterialPath = "Materials/layerTransitionMaterial";
        private const string MaterialFieldColor =     "_Color";
        private const string MaterialFieldMaintex =   "_MainTex";
        private const string MaterialFieldBlendtex =  "_BlendTex";
        private const string MaterialFieldBlend =     "_Blend";
        private const string MaterialFieldAlpha =     "_Alpha";
        public RawImage Renderer;

        private GraphicLayer _layer;

        public bool IsVideo => Video != null;
        public VideoPlayer Video;
        public AudioSource Audio;

        public string GraphicPath;
        public string GraphicName { get; private set; }

        private Coroutine _coFadingIn;
        private Coroutine _coFadingOut;

        public GraphicObject(GraphicLayer layer, string graphicPath, Texture tex)
        {
            GraphicPath = graphicPath;
            _layer = layer;

            GameObject ob = new GameObject();
            ob.transform.SetParent(layer.Panel);
            Renderer = ob.AddComponent<RawImage>();

            GraphicName = tex.name;

            InitGraphic();

            Renderer.name = string.Format(NameFormat, GraphicName);
            Renderer.material.SetTexture(MaterialFieldMaintex, tex);
        }

        public GraphicObject(GraphicLayer layer, string graphicPath, VideoClip clip, bool useAudio)
        {
            GraphicPath = graphicPath;
            _layer = layer;

            GameObject ob = new GameObject();
            ob.transform.SetParent(layer.Panel);
            Renderer = ob.AddComponent<RawImage>();

            GraphicName = clip.name;
            Renderer.name = string.Format(NameFormat, GraphicName);

            InitGraphic();

            RenderTexture tex = new RenderTexture(Mathf.RoundToInt(clip.width), Mathf.RoundToInt(clip.height), 0);
            Renderer.material.SetTexture(MaterialFieldMaintex, tex);

            Video = Renderer.gameObject.AddComponent<VideoPlayer>();
            Video.playOnAwake = true;
            Video.source = VideoSource.VideoClip;
            Video.clip = clip;
            Video.renderMode = VideoRenderMode.RenderTexture;
            Video.targetTexture = tex;
            Video.isLooping = true;

            Video.audioOutputMode = VideoAudioOutputMode.AudioSource;
            Audio = Video.gameObject.AddComponent<AudioSource>();

            Audio.volume = 0;
            if (!useAudio)
                Audio.mute = true;

            Video.SetTargetAudioSource(0, Audio);

            Video.frame = 0;
            Video.Prepare();
            Video.Play();

            Video.enabled = false;
            Video.enabled = true;
        }

        private void InitGraphic()
        {
            Renderer.transform.localPosition = Vector3.zero;
            Renderer.transform.localScale = Vector3.one;

            RectTransform rect = Renderer.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.one;

            Renderer.material = GetTransitionMaterial();

            Renderer.material.SetFloat(MaterialFieldBlend, 0);
            Renderer.material.SetFloat(MaterialFieldAlpha, 0);
        }

        private Material GetTransitionMaterial()
        {
            Material mat = Resources.Load<Material>(MaterialPath);

            if (mat != null)
                return new Material(mat);

            return null;
        }

        public Coroutine FadeIn(float speed = 1f, Texture blend = null)
        {
            if (_coFadingOut != null)
                PanelManager.StopCoroutine(_coFadingOut);

            if (_coFadingIn != null)
                return _coFadingIn;

            _coFadingIn = PanelManager.StartCoroutine(Fading(1f, speed, blend));

            return _coFadingIn;
        }

        public Coroutine FadeOut(float speed = 1f, Texture blend = null)
        {
            if (_coFadingIn != null)
                PanelManager.StopCoroutine(_coFadingIn);

            if (_coFadingOut != null)
                return _coFadingOut;

            _coFadingOut = PanelManager.StartCoroutine(Fading(0f, speed, blend));

            return _coFadingOut;
        }

        private IEnumerator Fading(float target, float speed, Texture blend)
        {
            bool isBlending = blend != null;
            bool fadingIn = target > 0;

            Renderer.material.SetTexture(MaterialFieldBlendtex, blend);
            Renderer.material.SetFloat(MaterialFieldAlpha, isBlending ? 1 : fadingIn ? 0 : 1);
            Renderer.material.SetFloat(MaterialFieldBlend, isBlending ? fadingIn ? 0 : 1 : 1);

            string opacityParam = isBlending ? MaterialFieldBlend : MaterialFieldAlpha;

            while (Renderer.material.GetFloat(opacityParam) != target)
            {
                float opacity = Mathf.MoveTowards(Renderer.material.GetFloat(opacityParam), target, speed * GraphicPanelManager.DefaultTransitionSpeed * Time.deltaTime);
                Renderer.material.SetFloat(opacityParam, opacity);

                if (IsVideo)
                    Audio.volume = opacity;

                yield return null;
            }

            _coFadingIn = null;
            _coFadingOut = null;

            if (target == 0)
                Destroy();
            else
                DestroyBackgroundGraphicsOnLayer();
        }

        private void Destroy()
        {
            if (_layer.CurrentGraphic != null && _layer.CurrentGraphic.Renderer == Renderer)
                _layer.CurrentGraphic = null;

            Object.Destroy(Renderer.gameObject);
        }

        private void DestroyBackgroundGraphicsOnLayer()
        {
            _layer.DestroyOldGraphics();
        }
    }
}
