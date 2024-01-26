using Modules.Core.Abstract.Model;
using Modules.Core.Concrete.Model;
using Modules.SRDebuggerAndCamera.Controller;
using Modules.SRDebuggerAndCamera.Model;
using Modules.SRDebuggerAndCamera.Signals;
using Modules.SRDebuggerAndCamera.View;
using Rich.Base.Runtime.Abstract.Model;
using Rich.Base.Runtime.Concrete.Model;
using UnityEngine;
using RichMVCContext = Rich.Base.Runtime.Concrete.Context.RichMVCContext;

namespace Modules.SRDebuggerAndCamera.Context
{
    public class SR_SampleContext : RichMVCContext
    {
        private SR_ExampleGameSignals _gameSignals;
        private BaseSrSignals _cam;

        public SR_SampleContext(GameObject root) : base(root)
        {
        }

        public SR_SampleContext()
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();
            //Base bindings 
            injectionBinder.Bind<SR_ExampleGameSignals>().CrossContext().ToSingleton();
            injectionBinder.Bind<IGameModel>().To<GameModel>().CrossContext().ToSingleton();
            injectionBinder.Bind<BaseSrSignals>().CrossContext().ToSingleton();
            
            mediationBinder.Bind<CinemachineView>().To<CinemachineMediator>();
            //Base End
            injectionBinder.Bind<ProjectSRSignals>().CrossContext().ToSingleton();
            injectionBinder.Bind<SRoptionsProject>().CrossContext().ToSingleton();
            _gameSignals = injectionBinder.GetInstance<SR_ExampleGameSignals>();
            // Bind SrDebugger commands to your initalize signal;
            commandBinder.Bind(_gameSignals.start).InSequence().To<SRDebuggerFixCommand>()
                .To<SROptionsModelRegisterCommand<SRoptionsProject>>();
        }

        public override void Launch()
        {
            base.Launch();
            _gameSignals.start.Dispatch();
        }
    }
    
}