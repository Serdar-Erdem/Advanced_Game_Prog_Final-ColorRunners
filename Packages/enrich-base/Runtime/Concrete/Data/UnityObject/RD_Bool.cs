using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Boolean", order = 3)]
    public class RD_Bool : ScriptableObject
    {
        public bool Value;
    }
}