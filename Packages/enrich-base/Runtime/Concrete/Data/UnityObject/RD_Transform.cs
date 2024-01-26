using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Transform", order = 5)]
    public class RD_Transform : ScriptableObject
    {
        public Transform Value;
    }
}