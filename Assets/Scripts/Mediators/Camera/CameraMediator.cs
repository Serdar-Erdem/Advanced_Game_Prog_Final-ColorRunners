using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Runtime.Views.Camera;
using Runtime.Signals;


namespace Runtime.Mediators.Camera
{
    public class CameraMediator : MediatorLite
    {
        [Inject] public CameraView View { get; set; }
        [Inject] public CameraSignals CameraSignals { get; set; }
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            CameraSignals.onSetCameraTarget += OnSetCameraTarget;
            CoreGameSignals.onReset += View.OnReset;
        }

        private void OnSetCameraTarget()
        {
            View.AssignCameraTarget();
        }

        public override void OnRemove()
        {
            base.OnRemove();

            CameraSignals.onSetCameraTarget -= OnSetCameraTarget;
            CoreGameSignals.onReset -= View.OnReset;
        }
    }
}
