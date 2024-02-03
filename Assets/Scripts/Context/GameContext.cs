using Rich.Base.Runtime.Concrete.Context;
using Rich.Base.Runtime.Extensions;
using Runtime.Controller;
using Runtime.Controller.LevelControllers;
using Runtime.Controller.PlayerControllers;
using Runtime.Mediators.Input;
using Runtime.Mediators.Pool;
using Runtime.Mediators.Camera;
using Runtime.Mediators.Player;
using Runtime.Model.Input;
using Runtime.Model.Level;
using Runtime.Model.Player;
using Runtime.Signals;
using Runtime.Views.Input;
using Runtime.Views.Pool
using Runtime.Views.Camera;
using Runtime.Views.Player;


namespace Runtime.Context
{
    public class GameContext : RichMVCContext
    {
        private InputSignals _inputSignals;
        private LevelSignals _levelSignals;
        private CoreGameSignals _coreGameSignals;
        private CameraSignals _cameraSignals;
        private PlayerSignals _playerSignals;


        protected override void mapBindings()
        {
            base.mapBindings();

            _coreGameSignals = injectionBinder.BindCrossContextSingletonSafely<CoreGameSignals>();
            _cameraSignals = injectionBinder.BindCrossContextSingletonSafely<CameraSignals>();
            _playerSignals = injectionBinder.BindCrossContextSingletonSafely<PlayerSignals>();
            _inputSignals = injectionBinder.BindCrossContextSingletonSafely<InputSignals>();
            _levelSignals = injectionBinder.BindCrossContextSingletonSafely<LevelSignals>();

            injectionBinder.Bind<ILevelModel>().To<LevelModel>().CrossContext().ToSingleton();
            injectionBinder.Bind<IInputModel>().To<InputModel>().CrossContext().ToSingleton();
            injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().CrossContext().ToSingleton();



            mediationBinder.BindView<InputView>().ToMediator<InputMediator>();
            mediationBinder.BindView<PoolControllerView>().ToMediator<PoolControllerMediator>();
            mediationBinder.BindView<CameraView>().ToMediator<CameraMediator>();
            mediationBinder.BindView<PlayerView>().ToMediator<PlayerMediator>();


            commandBinder.Bind(_levelSignals.onInitializeLevel).To<InitializeLevelCommand>();
            commandBinder.Bind(_levelSignals.onDestroyLevel).To<DestroyLevelCommand>();
            commandBinder.Bind(_levelSignals.onRestartLevel).To<LevelFailedCommand>();
            commandBinder.Bind(_levelSignals.onNextLevel).To<LevelSuccessfulCommand>();

            commandBinder.Bind(_coreGameSignals.onLevelFailed).To<OpenLevelFailedScreenCommand>();
            commandBinder.Bind(_coreGameSignals.onLevelSuccessful).To<OpenLevelSuccessfulScreenCommand>();
            commandBinder.Bind(_coreGameSignals.onReset).To<OpenStartScreenPanelCommand>();

            commandBinder.Bind(_playerSignals.onForceCommand).To<ForceBallsToPoolCommand>();
            commandBinder.Bind(_playerSignals.onStageAreaSuccessful).To<OnStageAreaSuccessfulCommand>();
        }

        public override void Launch()
        {
            base.Launch();
            _levelSignals.onInitializeLevel.Dispatch();
        }
    }
}