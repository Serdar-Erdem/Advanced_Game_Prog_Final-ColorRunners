using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rich.Base.Runtime.Abstract.Injectable.Provider
{
    public interface IUpdateProvider
    {
        void AddUpdate(UnityAction action);
        void RemoveUpdate(UnityAction action);

        void AddFixedUpdate(UnityAction action);
        void RemoveFixedUpdate(UnityAction action);

        void AddLateUpdate(UnityAction action);
        void RemoveLateUpdate(UnityAction action);
        
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);

        void StopAllCoroutines();
    }
}