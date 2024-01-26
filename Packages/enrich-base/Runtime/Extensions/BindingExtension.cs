using strange.extensions.injector.api;

namespace Rich.Base.Runtime.Extensions
{
    public static class BindingExtension
    {
        public static TAbstract BindCrossContextSingletonSafely<TAbstract, TConcrete>(this ICrossContextInjectionBinder injectionBinder)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TAbstract>();
            if (binding == null)
            {
                injectionBinder.Bind<TAbstract>().To<TConcrete>().ToSingleton().CrossContext();
            }
        
            TAbstract instance = injectionBinder.GetInstance<TAbstract>();
            return instance;
        }
        public static TDirect BindCrossContextSingletonSafely<TDirect>(this ICrossContextInjectionBinder injectionBinder)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TDirect>();
            if (binding == null)
            {
                injectionBinder.Bind<TDirect>().ToSingleton().CrossContext();
            }
        
            TDirect instance = injectionBinder.GetInstance<TDirect>();

            return instance;
        }
    
        public static TDirect BindCrossContextSingletonSafely<TDirect>(this ICrossContextInjectionBinder injectionBinder, object toObject)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TDirect>();
            if (binding == null)
            {
                injectionBinder.Bind<TDirect>().To(toObject).ToSingleton().CrossContext();
            }
        
            TDirect instance = injectionBinder.GetInstance<TDirect>();

            return instance;
        }
    }
}