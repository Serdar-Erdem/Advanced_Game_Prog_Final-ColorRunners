using Rich.Base.Runtime.Abstract.Injectable.Service.Backend;
using Rich.Base.Runtime.Abstract.Model;
using Rich.Base.Runtime.Concrete.Injectable.Controller;
using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Rich.Base.Runtime.Concrete.Injectable.Service.Backend;
using Rich.Base.Runtime.Concrete.Model;
using Rich.Base.Runtime.Concrete.Model.UnityObject;
using Rich.Base.Runtime.Concrete.View;
using Rich.Base.Runtime.Extensions;
using Rich.Base.Runtime.Signals;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Context
{
    public class BaseUIContext : RichMVCContext
    {
        public BaseUIContext(GameObject root) : base(root)
        {
            Debug.Log("UI Canvas Context Created");
        }

        public BaseUIContext()
        {
            
        }
    
        protected CoreScreenSignals CoreScreenSignals;

        protected override void mapBindings()
        {
            base.mapBindings();
            CoreScreenSignals = injectionBinder.BindCrossContextSingletonSafely<CoreScreenSignals>();
            injectionBinder.BindCrossContextSingletonSafely<BackendServiceSignals>();

            injectionBinder.BindCrossContextSingletonSafely<IScreenModel, ScreenModel>();
            injectionBinder.BindCrossContextSingletonSafely<IBundleModel, BundleModel>();
            injectionBinder.BindCrossContextSingletonSafely<IBackendService, BackendService>();
        
            //Binding screen manager.
            mediationBinder.Bind<RichScreenManagerView>().To<RichScreenManagerMediator>();
        
            //Commands necessary to open Panels.
            commandBinder.Bind(CoreScreenSignals.RetrievePanel).To<RetrieveScreenPanelFromResourcesCommand>();
            commandBinder.Bind(CoreScreenSignals.OpenPanel).To<OpenNormalPanelCommand>();
        }
    
    }
}