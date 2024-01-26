using System.Collections;
using Rich.Base.Runtime.Abstract.Injectable.Provider;
using UnityEngine;

namespace Rich.Base.Runtime.Abstract.Injectable.Process
{
    /// <summary>
    /// These types of updates must do something by a very specifically defined flow. If you need to cancel, you can just use ProcessProvider to return. it will cancel the coroutine.
    /// </summary>
    public abstract class CoroutineProcess<T> : ContinuousProcess where T : CoroutineProcess<T>
    {
        [Inject]
        public IUpdateProvider UpdateProvider { get; set; }
        protected abstract IEnumerator Routine();
        
        public bool AutoReturn;
        private IEnumerator Enumerator()
        {
            yield return Routine();
            if (AutoReturn)
            {
                bool success = ProcessProvider.Return(this as T);
                Debug.Log("Return successful = " + success);
            }
        }

        protected override void DoStart()
        {
            UpdateProvider.StartCoroutine(Enumerator());
        }
    }
}