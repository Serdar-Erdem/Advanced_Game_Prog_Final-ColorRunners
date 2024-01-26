using Enums;
using strange.extensions.signal.impl;
using Extentions;

namespace Signals
{
    [Inject]
    public class UISignals : MonoSingleton<UISignals>
    {
        private Signal<UIPanelTypes> onOpenPanelSignal = new Signal<UIPanelTypes>();
        private Signal<UIPanelTypes> onClosePanelSignal = new Signal<UIPanelTypes>();

        public Signal<UIPanelTypes> onOpenPanel
        {
            get { return onOpenPanelSignal; }
        }

        public Signal<UIPanelTypes> onClosePanel
        {
            get { return onClosePanelSignal; }
        }

        public void InvokeOnOpenPanel(UIPanelTypes panelType)
        {
            onOpenPanelSignal.Dispatch(panelType);
        }

        public void InvokeOnClosePanel(UIPanelTypes panelType)
        {
            onClosePanelSignal.Dispatch(panelType);
        }
    }
}
```