using strange.extensions.signal.impl;
using Extentions;

namespace Signals
{
    [Inject]
    public class TurretSignals : MonoSingleton<TurretSignals>
    {
        private Signal onResetListSignal = new Signal();

        public Signal onResetList
        {
            get { return onResetListSignal; }
        }

        public void InvokeOnResetList()
        {
            onResetListSignal.Dispatch();
        }
    }
}
