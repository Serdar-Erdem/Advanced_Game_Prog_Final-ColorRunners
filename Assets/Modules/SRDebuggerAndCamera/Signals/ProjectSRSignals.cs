
using strange.extensions.signal.impl;

namespace Modules.SRDebuggerAndCamera.Signals
{
    public class ProjectSRSignals 
    {
        public Signal Ending  = new Signal();
        public Signal CloseBackground  = new Signal();
        public Signal OpenBackground = new Signal();
        public Signal TurnOffEffects = new Signal();
        public Signal TurnOnEffects = new Signal();
    }
}
