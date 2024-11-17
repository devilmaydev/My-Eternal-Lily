using UnityEngine;

namespace _MAIN.Scripts.Extensions
{
    public class CoroutineWrapper
    {
        private MonoBehaviour _owner;
        private Coroutine _coroutine;

        public bool IsDone = false;

        public CoroutineWrapper(MonoBehaviour owner, Coroutine coroutine)
        {
            this._owner = owner;
            this._coroutine = coroutine;
        }

        public void Stop()
        {
            _owner.StopCoroutine(_coroutine);
            IsDone = true;
        }
    }
}