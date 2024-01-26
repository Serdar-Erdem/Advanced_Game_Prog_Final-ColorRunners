using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/String Dictionary", order = 6)]
    public class RD_StringDictionary : SerializedScriptableObject
    {
        public Dictionary<string, string> List;
    }
}