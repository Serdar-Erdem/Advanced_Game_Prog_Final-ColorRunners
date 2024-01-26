using strange.framework.api;

namespace Rich.Base.Runtime.Abstract.Injectable.Binder
{
	public interface IScriptableObjectBinder : IBinder
    {
        void Inject(IInjectableScriptableObject injectable);
    }
}