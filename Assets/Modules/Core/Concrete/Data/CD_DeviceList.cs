using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Core.Concrete.Data
{
    [CreateAssetMenu(menuName = "Runtime Data/Device List", order = 11)]
    public class CD_DeviceList : ScriptableObject
    {
        public List<DeviceVo> List;
    }

    [Serializable]
    public struct DeviceVo
    {
        public string Username;
        public string Model;
        public string SerialNumber;
        public string IDFA;
    }
}