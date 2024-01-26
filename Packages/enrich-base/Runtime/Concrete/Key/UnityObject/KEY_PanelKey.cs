using Rich.Base.Runtime.Abstract.Key;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Key.UnityObject
{
    //Unity side
    [CreateAssetMenu(fileName = "PanelKey_", menuName = "Rich/Keys/Panel Key")]
    public class KEY_PanelKey :  ScriptableObject,IPanelKey
    {
        [SerializeField]
        private string _resourceName;
        public string ResourceName
        {
            get => _resourceName;
            set => _resourceName = value;
        }
    }
}