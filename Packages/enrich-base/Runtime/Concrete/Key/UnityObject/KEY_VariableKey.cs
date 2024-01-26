using Rich.Base.Runtime.Abstract.Key;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Key.UnityObject
{
    [CreateAssetMenu(fileName = "VariableKey", menuName = "Rich/Keys/Variable Key")]
    public class KEY_VariableKey : ScriptableObject,IVariableKey
    {
        [SerializeField]
        private string _id;
        public string ID
        {
            get => _id;
        }
    }
}