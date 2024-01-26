using Rich.Base.Runtime.Abstract.Key;
using Rich.Base.Runtime.Concrete.Data.ValueObject;
using Rich.Base.Runtime.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Controller
{
    public class OpenNormalPanelCommand : Command
    {
        [Inject]
        public CoreScreenSignals CoreScreenSignals { get; set; }
        
        [Inject]
        public OpenNormalPanelArgs ParamArgs { get; set; }
        
        public override void Execute()
        {
            Debug.Log("Opening Normal Panel " + ParamArgs.PanelKey);
            PanelVo panelVo = new PanelVo()
            {
                Key = ParamArgs.PanelKey,
                LayerIndex = ParamArgs.LayerIndex,
                Name = ParamArgs.PanelKey,
                IgnoreHistory = ParamArgs.IgnoreHistory
            };
            CoreScreenSignals.DisplayPanel.Dispatch(panelVo);
        }
    }
}