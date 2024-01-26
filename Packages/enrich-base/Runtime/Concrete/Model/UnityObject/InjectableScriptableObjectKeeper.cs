using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Injectable;
using Rich.Base.Runtime.Concrete.Root;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Model.UnityObject
{
    public class InjectableScriptableObjectKeeper : MonoBehaviour
    {
        [SerializeField] private InjectableScriptableObjectRoot _scriptableObjectRoot;
        
        [SerializeField] private List<ScriptableObject> _scriptableObjects;
        private List<IInjectableScriptableObject> _injectableScriptableObjects;
        
        protected virtual void Start()
        {
            Debug.Log("Filtering scriptable objects...");
            if (_injectableScriptableObjects == null)
            {
                _injectableScriptableObjects = new List<IInjectableScriptableObject>();
                foreach (ScriptableObject obj in _scriptableObjects)
                {
                    if (obj is IInjectableScriptableObject)
                    {
                        _injectableScriptableObjects.Add(obj as IInjectableScriptableObject);
                    }
                }
            }

            Debug.Log("Requesting injection from the context...");
            foreach (IInjectableScriptableObject injectable in _injectableScriptableObjects)
            {
                _scriptableObjectRoot.Inject(injectable);
            }
        }
    }
}