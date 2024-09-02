using System.Collections;
using _MAIN.Scripts.Core.IO;
using UnityEngine;

namespace _TESTING.Scripts
{
    public class TestFiles : MonoBehaviour
    {
        [SerializeField] private TextAsset fileName;

        private void Start() => StartCoroutine(Run());

        private IEnumerator Run()
        {
            var lines = FileManager.ReadTextAsset(fileName, false);
            foreach (var line in lines)
                Debug.Log(line);
        
            yield return null;
        }
    }
}
