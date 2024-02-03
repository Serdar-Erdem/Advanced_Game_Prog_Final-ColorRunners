using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Runtime.Views.Screen;
using Runtime.Signals;


namespace Runtime.Mediators.Screen
{
    public class WinScreenMediator : MediatorLite
    {
        [Inject] public WinScreenView View { get; set; }
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }
        [Inject] public CoreScreenSignals CoreScreenSignals { get; set; }
        [Inject] public LevelSignals LevelSignals { get; set; }


        public override void OnRegister()
        {
            base.OnRegister();
            View.onNextLevel += OnNextLevel;
        }


        private void OnNextLevel()
        {
            LevelSignals.onNextLevel.Invoke();
            CoreGameSignals.onReset.Invoke();
            CoreScreenSignals.ClearLayerPanel.Invoke(3);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            View.onNextLevel -= OnNextLevel;
        }
    }
}
