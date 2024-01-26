using Modules.Core.Concrete.Data;
using Rich.Base.Runtime.Concrete.Data.UnityObject;

namespace Modules.Core.Abstract.Model
{
    /// <summary>
    /// app related datas
    /// </summary>
    public interface IGameModel
    {
        RD_GameStatus Status { get; }
        CD_DeviceList TestDeviceList { get; }
        bool IsEnableSrDebugger{ get; set; }
        void Clear();
    }
}