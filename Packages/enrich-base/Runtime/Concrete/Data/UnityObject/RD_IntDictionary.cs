using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Int Dictionary", order = 7)]
    public class RD_IntDictionary : SerializedScriptableObject
    {
        public Dictionary<string, int> List;
    }
}