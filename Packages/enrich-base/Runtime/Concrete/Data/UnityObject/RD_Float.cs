using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Float", order = 2)]
    public class RD_Float : ScriptableObject
    {
        public float Value;
    }
}