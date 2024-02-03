using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Runtime.Views.Screen;
using Runtime.Signals;


namespace Runtime.Mediators.Screen
{
    public class StartScreenMediator : MediatorLite
    {
        [Inject] public StartScreenView View { get; set; }
        [Inject] public CameraSignals CameraSignals { get; set; }
        [Inject] public CoreScreenSignals CoreScreenSignals { get; set; }
        [Inject] public InputSignals InputSignals { get; set; }
        [Inject] public UISignals UISignals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            View.onPlay += OnPlay;
        }

        private void OnPlay()
        {
            CameraSignals.onSetCameraTarget.Invoke();
            CoreScreenSignals.ClearLayerPanel.Invoke(0);
            InputSignals.onEnableInput.Invoke();
            UISignals.onPlay.Invoke();

        }

        public override void OnRemove()
        {
            base.OnRemove();
            View.onPlay -= OnPlay;
        }
    }
}
