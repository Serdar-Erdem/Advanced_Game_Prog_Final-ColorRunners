using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Injectable.Provider;
using Rich.Base.Runtime.Concrete.Injectable.Process;
using strange.extensions.command.impl;

namespace Rich.Base.Runtime.Concrete.Injectable.Controller
{
    public class RetrieveScreenPanelFromResourcesCommand : Command
    {
        [Inject] public IPanelVo ParamPanelVo{get;set;} //Set by signal
    
        [Inject] public IProcessProvider ProcessProvider{get;set;}

        public override void Execute()
        {
            RetrieveResourcesPanelProcess process = ProcessProvider.Get<RetrieveResourcesPanelProcess>();
            process.PanelVo = ParamPanelVo;
            process.AutoReturn = true;
            process.Start();
        }
    }
}