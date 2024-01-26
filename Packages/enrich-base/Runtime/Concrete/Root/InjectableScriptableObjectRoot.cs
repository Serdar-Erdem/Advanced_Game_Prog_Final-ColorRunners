using Rich.Base.Runtime.Abstract.Injectable;
using Rich.Base.Runtime.Concrete.Context;
using strange.extensions.context.impl;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Root
{
    public class InjectableScriptableObjectRoot : ContextView
    {
        private bool _isInitialized;
        private InjectableScriptableObjectContext _context;
        public void Inject(IInjectableScriptableObject injectableScriptableObject)
        {
            ValidateInitialization();
            _context.Inject(injectableScriptableObject);
        }

        private void ValidateInitialization()
        {
            if (_isInitialized) return;
            Debug.Log("Initializing InjectableScriptableObjectContext");
            _context = new InjectableScriptableObjectContext();
            context = _context;
            _isInitialized = true;
        }
    }
}