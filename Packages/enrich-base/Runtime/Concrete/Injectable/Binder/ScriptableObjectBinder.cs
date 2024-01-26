using Rich.Base.Runtime.Abstract.Injectable;
using Rich.Base.Runtime.Abstract.Injectable.Binder;
using strange.extensions.injector.api;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Binder
{
    public class ScriptableObjectBinder : strange.framework.impl.Binder, IScriptableObjectBinder
    {
        [Inject]
        public IInjectionBinder injectionBinder { get; set; }
        public void Inject(IInjectableScriptableObject injectable)
        {
            InjectRequired(injectable);
        }

        /// Inject by a keeper, this is required since scriptable objects doesn't exist in a scene, they rather exist in whole Project.
        protected virtual void InjectRequired(IInjectableScriptableObject injectable)
        {
            if (injectable != null && injectable.shouldRegister)
            {
                if (injectable.autoRegisterWithContext && injectable.registeredWithContext)
                {
                    Debug.Log("This scriptable object already registered with context [" + injectable.name + "]");
                    return;
                }
                Debug.Log("Injecting to scriptable object...");
                injectionBinder.injector.Inject(injectable, false);
                injectable.registeredWithContext = true;
            }
        }
    }
}