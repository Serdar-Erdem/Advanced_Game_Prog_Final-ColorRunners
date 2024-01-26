using Rich.Base.Runtime.Abstract.Function;
using Rich.Base.Runtime.Concrete.Data.ValueObject;

//using Service.Localization;

namespace Rich.Base.Runtime.Concrete.Injectable.Mediator
{
    public class ConfirmPanelMediator : strange.extensions.mediation.impl.Mediator
    {
        /// <summary>
        /// View injection
        /// </summary>
        [Inject]
        public IConfirmPanel view { get; set; }

        //[Inject]
        //public ILocalizationService localizationService { get; set; }

        /// <summary>
        /// Panel details
        /// </summary>
        private ConfirmPanelVo vo
        {
            get { return view.vo as ConfirmPanelVo; }
        }

        /// <summary>
        /// Works after all bindings are completed. 
        /// Useful to attach listeners
        /// After Awake 
        /// Before Start. 
        /// </summary>
        public override void OnRegister()
        {
            // add view listeners
            view.onConfirm += OnConfirm;
            view.onCancel += OnCancel;

            //view.Title = localizationService.GetText(vo.Title);
            //view.Description = localizationService.GetText(vo.Description);
            //view.ConfirmButtonLabel = localizationService.GetText(vo.ButtonLabel);
            //view.CancelButtonLabel = localizationService.GetText(vo.CancelButtonLabel);
            view.IconName = vo.Icon;
        }

        /// <summary>
        /// Confirm event callback
        /// </summary>
        private void OnConfirm()
        {
            if (vo.OnConfirm != null)
                vo.OnConfirm();
            Destroy(gameObject);
        }

        /// <summary>
        /// Cancel event callback
        /// </summary>
        private void OnCancel()
        {
            if (vo.OnCancel != null)
                vo.OnCancel();
            Destroy(gameObject);
        }

        /// <summary>
        /// Works when connected gameobject is destroyed. 
        /// Useful to remove listeners
        /// Before OnDestroy method
        /// </summary>
        public override void OnRemove()
        {
            // remove view listeners
            view.onConfirm -= OnConfirm;
            view.onCancel -= OnCancel;
        }
    }
}