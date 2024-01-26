using strange.extensions.mediation.api;
using UnityEngine.Events;

namespace Rich.Base.Runtime.Abstract.Function
{
    public enum ConfirmPanelEvent
    {
        Cancel,
        Confirm
    }

    public interface IConfirmPanel : IPanel, IView
    {
        /// <summary>
        /// Title of confirm panel
        /// </summary>
        string Title { set; }

        /// <summary>
        /// Description of confirm panel
        /// </summary>
        string Description { set; }

        /// <summary>
        /// Confirm button text
        /// </summary>
        string ConfirmButtonLabel { set; }

        /// <summary>
        /// Cancel button text
        /// </summary>
        string CancelButtonLabel { set; }

        /// <summary>
        /// Icon name on resources folder
        /// </summary>
        string IconName { set; }
        
        event UnityAction onConfirm;
        
        event UnityAction onCancel;
    }
}