using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Runtime.Views.Screen;
using Runtime.Signals;


namespace Runtime.Mediators.Screen
{
    public class LevelScreenMediator : MediatorLite
    {
        [Inject] public LevelScreenView View { get; set; }
        [Inject] public UISignals UISignals { get; set; }


        public override void OnRegister()
        {
            base.OnRegister();
            UISignals.onSetStageColor += View.OnSetStageColor;
            UISignals.onSetNewLevelValue += View.OnSetLevelValue;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            UISignals.onSetStageColor -= View.OnSetStageColor;
            UISignals.onSetNewLevelValue -= View.OnSetLevelValue;
        }
    }
}
