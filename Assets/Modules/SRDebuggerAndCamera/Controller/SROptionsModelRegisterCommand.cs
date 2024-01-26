using Modules.Core.Abstract.Model;
using Rich.Base.Runtime.Abstract.Model;
using strange.extensions.command.impl;

namespace Modules.SRDebuggerAndCamera.Controller
{
    public class SROptionsModelRegisterCommand<TModel> : Command where TModel : class
    {
        [Inject]
        public IGameModel GameModel { get; set; }
        [Inject]
        public TModel Model { get; set; }
        public override void Execute()
        {
            if (GameModel.IsEnableSrDebugger)
            {
                SRDebug.Instance.AddOptionContainer(Model);
            }
        }
    }
}