using strange.extensions.signal.impl;
using Extentions;

namespace Signals
{
    [Inject]
    public class InputSignals : MonoSingleton<InputSignals>
    {
        private Signal<RunnerHorizontalInputParams> onInputDraggedSignal = new Signal<RunnerHorizontalInputParams>();
        private Signal<IdleInputParams> onIdleInputTakenSignal = new Signal<IdleInputParams>();
        private Signal<bool> onSidewaysEnableSignal = new Signal<bool>();

        public Signal<RunnerHorizontalInputParams> onInputDragged
        {
            get { return onInputDraggedSignal; }
        }

        public Signal<IdleInputParams> onIdleInputTaken
        {
            get { return onIdleInputTakenSignal; }
        }

        public Signal<bool> onSidewaysEnable
        {
            get { return onSidewaysEnableSignal; }
        }

        public void InvokeOnInputDragged(RunnerHorizontalInputParams inputParams)
        {
            onInputDraggedSignal.Dispatch(inputParams);
        }

        public void InvokeOnIdleInputTaken(IdleInputParams inputParams)
        {
            onIdleInputTakenSignal.Dispatch(inputParams);
        }

        public void InvokeOnSidewaysEnable(bool enable)
        {
            onSidewaysEnableSignal.Dispatch(enable);
        }
    }
}
```