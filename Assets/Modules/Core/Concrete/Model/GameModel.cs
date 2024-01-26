using Modules.Core.Abstract.Model;
using Modules.Core.Concrete.Data;
using Rich.Base.Runtime.Concrete.Data.UnityObject;
using UnityEngine;

namespace Modules.Core.Concrete.Model
{
    public class GameModel : IGameModel
    {
        private RD_GameStatus _status;
        
        private CD_DeviceList _deviceList;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _status = Resources.Load<RD_GameStatus>("Data/AppStatus");
            
            _deviceList = Resources.Load<CD_DeviceList>("Data/TestDeviceList");

        }

        public RD_GameStatus Status
        {
            get
            {
                if (_status == null)
                    OnPostConstruct();

                return _status;
            }
        }
        public CD_DeviceList TestDeviceList
        {
            get
            {
                if (_deviceList == null)
                    OnPostConstruct();

                return _deviceList;
            }
        }

        public bool IsEnableSrDebugger { get; set; }

        public void Clear()
        {
        }
    }
}