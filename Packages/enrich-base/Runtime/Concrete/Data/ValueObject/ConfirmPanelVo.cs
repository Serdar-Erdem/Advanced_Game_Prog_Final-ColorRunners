using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Key;
using Rich.Base.Runtime.Enums;

namespace Rich.Base.Runtime.Concrete.Data.ValueObject
{
    public class ConfirmPanelVo : IPanelVo
    {
        /// <summary>
        /// Title text
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description text
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Confirm text
        /// </summary>
        public string ButtonLabel { get; set; }

        /// <summary>
        /// Cancel text
        /// </summary>
        public string CancelButtonLabel { get; set; }

        /// <summary>
        /// Icon name on resources folder
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Name of panel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Layer of panel
        /// </summary>
        public int LayerIndex { get; set; }

        /// <summary>
        /// Is this panel need to clear all layers
        /// </summary>
        public bool RemoveAll { get; set; }

        /// <summary>
        /// Is this panel need to remove same panels
        /// </summary>
        public bool RemoveSamePanels { get; set; }

        public bool IgnoreHistory
        {
            get;
            set;
        }

        public string Key { get; set; }

        /// <summary>
        /// Is this panel need to clear layer
        /// </summary>
        public bool RemoveLayer { get; set; }

        /// <summary>
        /// Is this panel cancellable
        /// </summary>
        public bool NotCancellable { get; set; }

        /// <summary>
        /// Confirm callback function
        /// </summary>
        public PanelCallback OnConfirm { get; set; }

        /// <summary>
        /// Cancel callback function
        /// </summary>
        public PanelCallback OnCancel { get; set; }

        /// <summary>
        /// Type on AppStatus. Should be Confirm=1
        /// </summary>
       // public GameStatus Type { get; set; }
    }
}