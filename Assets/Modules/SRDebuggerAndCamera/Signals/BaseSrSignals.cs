using strange.extensions.signal.impl;
using UnityEngine;

namespace Modules.SRDebuggerAndCamera.Signals
{
    public class BaseSrSignals 
    {
        public Signal Reset = new Signal();
        public Signal<Vector2> UpdateCam  = new Signal<Vector2>();
        public Signal<float> UpdateFov  = new Signal<float>();
        public Signal<float> UpdateZoom  = new Signal<float>();
        public Signal FreeCam = new Signal();
        public Signal MainCam = new Signal();
    }
}
