using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Key;
using Rich.Base.Runtime.Enums;
using Sirenix.OdinInspector;

namespace Rich.Base.Runtime.Concrete.Data.ValueObject
{
    [HideReferenceObjectPicker]
    public class PanelVo : IPanelVo
    {
        public PanelVo()
        {
            LayerIndex = 0;
            RemoveAll = false;
            RemoveLayer = true;
            RemoveSamePanels = true;
        }

        [ShowInInspector]
        public string Name { get; set; }

        [ShowInInspector]
        public int LayerIndex { get; set; }

        [ShowInInspector]
        public bool RemoveAll { get; set; }

        [ShowInInspector]
        public bool RemoveLayer { get; set; }

        [ShowInInspector]
        public bool RemoveSamePanels { get; set; }

        [ShowInInspector]
        public bool NotCancellable { get; set; }

        public PanelCallback OnCancel { get; set; }
        
        [ShowInInspector]
        public string Key{get;set;}

        [ShowInInspector]
        public bool IgnoreHistory
        {
            get;
            set;
        }
    }
}