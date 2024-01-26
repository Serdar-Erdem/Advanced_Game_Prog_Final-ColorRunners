using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Vector3", order = 4)]
    public class RD_Vector3 : ScriptableObject
    {
        public Vector3 Value;
    }
}