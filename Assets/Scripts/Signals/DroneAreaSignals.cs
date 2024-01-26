using strange.extensions.signal.impl;
using Extentions;

namespace Signals
{
    [Inject]
    public class DroneAreaSignals : MonoSingleton<DroneAreaSignals>
    {
        private Signal onDroneCheckStartedSignal = new Signal();
        private Signal onDroneCheckCompletedSignal = new Signal();

        public Signal onDroneCheckStarted
        {
            get { return onDroneCheckStartedSignal; }
        }

        public Signal onDroneCheckCompleted
        {
            get { return onDroneCheckCompletedSignal; }
        }

        public void InvokeOnDroneCheckStarted()
        {
            onDroneCheckStartedSignal.Dispatch();
        }

        public void InvokeOnDroneCheckCompleted()
        {
            onDroneCheckCompletedSignal.Dispatch();
        }
    }
}
```