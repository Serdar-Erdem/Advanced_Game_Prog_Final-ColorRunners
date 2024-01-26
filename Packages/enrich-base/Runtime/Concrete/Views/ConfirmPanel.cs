using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Function;
using Rich.Base.Runtime.Abstract.View;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rich.Base.Runtime.Concrete.View
{
    public class ConfirmPanel : RichView, IConfirmPanel
    {
        public event UnityAction onConfirm;
        public event UnityAction onCancel;

        public override bool autoRegisterWithContext { get=>false; }

        /// <summary>
        /// Title object
        /// </summary>
        public TextMeshProUGUI TitleLabel;

        /// <summary>
        /// Description object
        /// </summary>
        public TextMeshProUGUI DescriptionLabel;

        /// <summary>
        /// Cancel button object
        /// </summary>
        public TextMeshProUGUI CancelButtonText;

        /// <summary>
        /// Confirm button object
        /// </summary>
        public TextMeshProUGUI ConfirmButtonText;

        /// <summary>
        /// Confirm panel icon
        /// </summary>
        public Image Icon;

        /// <summary>
        /// Cancel event listener
        /// </summary>
        public void OnCancelClick()
        {
            onCancel?.Invoke();
        }

        /// <summary>
        /// Confirm event listener
        /// </summary>
        public void OnConfirmClick()
        {
            onConfirm?.Invoke();
        }
        /// <summary>
        /// Title of confirm panel
        /// </summary>
        public string Title
        {
            set
            {
                if (TitleLabel != null)
                    TitleLabel.text = value;

            }
        }

        /// <summary>
        /// Description of confirm panel
        /// </summary>
        public string Description
        {
            set
            {
                if (DescriptionLabel != null)
                    DescriptionLabel.text = value;
            }
        }

        /// <summary>
        /// Confirm button text
        /// </summary>
        public string ConfirmButtonLabel
        {
            set
            {
                if (ConfirmButtonText != null)
                    ConfirmButtonText.text = value;
            }
        }

        /// <summary>
        /// Cancel button text
        /// </summary>
        public string CancelButtonLabel
        {
            set
            {
                if (CancelButtonText != null)
                    CancelButtonText.text = value;
            }
        }

        /// <summary>
        /// Icon name in resources folder
        /// </summary>
        public string IconName
        {
            set
            {
                if (Icon != null)
                {
                    Icon.sprite = Resources.Load<Sprite>("GUI/Sprites/" + value);
                    Icon.SetNativeSize();
                }
            }
        }

        /// <summary>
        /// Connected PanelVo
        /// </summary>
        public IPanelVo vo { get; set; }
    }
}
