using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Runtime.Model.Level;
using Runtime.Views.Pool;
using Runtime.Signals;


namespace Runtime.Mediators.Pool
{
    public class PoolControllerMediator : MediatorLite
    {
        [Inject] public PoolControllerView View { get; set; }
        [Inject] public PlayerSignals PlayerSignals { get; set; }
        [Inject] public ILevelModel LevelModel { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            PlayerSignals.onStageAreaSuccessful += OnStageAreaSuccessful;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            PlayerSignals.onStageAreaSuccessful -= OnStageAreaSuccessful;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            View.SetPoolData(LevelModel.LevelData[LevelModel.GetLevelValue()].PoolData[View.StageValue]);
        }

        private void OnStageAreaSuccessful(byte stageValue)
        {
            View.OnActivateTweens();
            View.OnChangePoolColor();
        }
    }
}
