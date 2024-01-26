using strange.extensions.signal.impl;
using Enums;
using Extentions;

namespace Signals
{
    [Inject]
    public class CameraSignals : MonoSingleton<CameraSignals>
    {
        private Signal<CameraStates> onChangeCameraStatesSignal = new Signal<CameraStates>();

        public Signal<CameraStates> onChangeCameraStates
        {
            get { return onChangeCameraStatesSignal; }
        }

        public void InvokeOnChangeCameraStates(CameraStates cameraState)
        {
            onChangeCameraStatesSignal.Dispatch(cameraState);
        }
    }
}
