using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Systems.Audio.Pooling
{
    /// <summary>
    /// Pool de AudioSource para melhor performance
    /// Evita criação/destruição constante de objetos
    /// </summary>
    public class AudioSourcePool : MonoBehaviour
    {
        private Queue<AudioSource> _pool = new();
        private List<AudioSource> _active = new();
        private Transform _poolRoot;
        
        [SerializeField] private int _initialPoolSize = 10;
        [SerializeField] private int _maxPoolSize = 50;
        
        private void Awake()
        {
            _poolRoot = new GameObject("AudioSourcePool").transform;
            _poolRoot.SetParent(transform);
            
            // Pre-populate pool
            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateNewAudioSource();
            }
        }
        
        /// <summary>
        /// Obtém um AudioSource do pool
        /// </summary>
        public AudioSource GetAudioSource()
        {
            AudioSource source;
            
            if (_pool.Count > 0)
            {
                source = _pool.Dequeue();
            }
            else
            {
                source = CreateNewAudioSource();
            }
            
            _active.Add(source);
            source.gameObject.SetActive(true);
            
            return source;
        }
        
        /// <summary>
        /// Retorna um AudioSource para o pool
        /// </summary>
        public void ReturnAudioSource(AudioSource source)
        {
            if (source == null) return;
            
            _active.Remove(source);
            
            // Reset source
            source.Stop();
            source.clip = null;
            source.volume = 1f;
            source.pitch = 1f;
            source.loop = false;
            source.outputAudioMixerGroup = null;
            
            // Return to pool or destroy if pool is full
            if (_pool.Count < _maxPoolSize)
            {
                _pool.Enqueue(source);
                source.gameObject.SetActive(false);
            }
            else
            {
                Destroy(source.gameObject);
            }
        }
        
        /// <summary>
        /// Cria um novo AudioSource
        /// </summary>
        private AudioSource CreateNewAudioSource()
        {
            var go = new GameObject($"AudioSource_{_pool.Count}");
            go.transform.SetParent(_poolRoot);
            go.SetActive(false);
            
            var source = go.AddComponent<AudioSource>();
            source.spatialBlend = 0; // 2D audio
            
            _pool.Enqueue(source);
            return source;
        }
        
        /// <summary>
        /// Limpa o pool
        /// </summary>
        public void ClearPool()
        {
            // Return all active sources
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                ReturnAudioSource(_active[i]);
            }
            
            // Destroy all pooled sources
            while (_pool.Count > 0)
            {
                var source = _pool.Dequeue();
                if (source != null)
                    Destroy(source.gameObject);
            }
        }
        
        /// <summary>
        /// Obtém estatísticas do pool
        /// </summary>
        public (int pooled, int active) GetPoolStats()
        {
            return (_pool.Count, _active.Count);
        }
    }
}
