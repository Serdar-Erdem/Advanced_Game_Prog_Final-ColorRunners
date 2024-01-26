using Modules.Core.Abstract.Model;
using Modules.Core.Concrete.Data;
using Rich.Base.Runtime.Abstract.Model;
using Rich.Base.Runtime.Concrete.Data.UnityObject;
using SRDebugger;
using strange.extensions.command.impl;
using UnityEngine;

namespace Modules.SRDebuggerAndCamera.Controller
{
    
    public class SRDebuggerFixCommand : Command
    {
        [Inject] public IGameModel Game { get; set; }
        public override void Execute()
        {
            Retain();
            Debug.Log("SrDebuggerFixCommand Execute");
#if UNITY_EDITOR
            Settings.Instance.IsEnabled = false;
            if (Application.isEditor)
            {
                Game.IsEnableSrDebugger = true;
                SRDebug.Init();
                Release();
                return;
            }
#endif
#if UNITY_ANDROID
                Release();
                return;
#endif
            Application.RequestAdvertisingIdentifierAsync(IdfaCheck);
        }
        private void IdfaCheck(string advertisingId, bool trackingEnabled, string error)
        {
            Debug.Log("SrDebuggerFixCommand --> IdfaCheck --> Execute");
            foreach (DeviceVo info in Game.TestDeviceList.List)
            {
                if (info.IDFA == advertisingId)
                {
                    Game.IsEnableSrDebugger = true;
                    SRDebug.Init();
                    break;
                }
            }
            Release();
        }
    }
}