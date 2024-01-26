using strange.extensions.signal.impl;
using Extentions;

namespace Signals
{
    [Inject]
    public class PlayerSignal : MonoSingleton<PlayerSignal>
    {
        private Signal<float> onChangeVerticalSpeedSignal = new Signal<float>();
        private Signal onIncreaseScaleSignal = new Signal();

        public Signal<float> onChangeVerticalSpeed
        {
            get { return onChangeVerticalSpeedSignal; }
        }

        public Signal onIncreaseScale
        {
            get { return onIncreaseScaleSignal; }
        }

        public void InvokeOnChangeVerticalSpeed(float speed)
        {
            onChangeVerticalSpeedSignal.Dispatch(speed);
        }

        public void InvokeOnIncreaseScale()
        {
            onIncreaseScaleSignal.Dispatch();
        }
    }
}
```