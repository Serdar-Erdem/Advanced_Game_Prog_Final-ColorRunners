using Extentions;
using Enums;
using strange.extensions.signal.impl;

namespace Signals
{
    [Inject]
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        private Signal<GameStates> onChangeGameStateSignal = new Signal<GameStates>();
        private Signal<GameStates> onGetGameStateSignal = new Signal<GameStates>();
        private Signal onGameInitSignal = new Signal();
        private Signal onGameInitLevelSignal = new Signal();
        private Signal onLevelInitializeSignal = new Signal();
        private Signal onLevelIdleInitializeSignal = new Signal();
        private Signal onClearActiveIdleLevelSignal = new Signal();
        private Signal onClearActiveLevelSignal = new Signal();
        private Signal onLevelFailedSignal = new Signal();
        private Signal onLevelSuccessfulSignal = new Signal();
        private Signal onNextLevelSignal = new Signal();
        private Signal onRestartLevelSignal = new Signal();
        private Signal onPlaySignal = new Signal();
        private Signal onResetSignal = new Signal();
        private Signal onStageAreaReachedSignal = new Signal();
        private Signal onStageSuccessfulSignal = new Signal();
        private Signal<int> onGetLevelIDSignal = new Signal<int>();
        private Signal<int> onGetIdleLevelIDSignal = new Signal<int>();

        public Signal<GameStates> onChangeGameState
        {
            get { return onChangeGameStateSignal; }
        }

        public Signal<GameStates> onGetGameState
        {
            get { return onGetGameStateSignal; }
        }

        public Signal onGameInit
        {
            get { return onGameInitSignal; }
        }

        public Signal onGameInitLevel
        {
            get { return onGameInitLevelSignal; }
        }

        public Signal onLevelInitialize
        {
            get { return onLevelInitializeSignal; }
        }

        public Signal onLevelIdleInitialize
        {
            get { return onLevelIdleInitializeSignal; }
        }

        public Signal onClearActiveIdleLevel
        {
            get { return onClearActiveIdleLevelSignal; }
        }

        public Signal onClearActiveLevel
        {
            get { return onClearActiveLevelSignal; }
        }

        public Signal onLevelFailed
        {
            get { return onLevelFailedSignal; }
        }

        public Signal onLevelSuccessful
        {
            get { return onLevelSuccessfulSignal; }
        }

        public Signal onNextLevel
        {
            get { return onNextLevelSignal; }
        }

        public Signal onRestartLevel
        {
            get { return onRestartLevelSignal; }
        }

        public Signal onPlay
        {
            get { return onPlaySignal; }
        }

        public Signal onReset
        {
            get { return onResetSignal; }
        }

        public Signal onStageAreaReached
        {
            get { return onStageAreaReachedSignal; }
        }

        public Signal onStageSuccessful
        {
            get { return onStageSuccessfulSignal; }
        }

        public Signal<int> onGetLevelID
        {
            get { return onGetLevelIDSignal; }
        }

        public Signal<int> onGetIdleLevelID
        {
            get { return onGetIdleLevelIDSignal; }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public void InvokeOnChangeGameState(GameStates gameState)
        {
            onChangeGameStateSignal.Dispatch(gameState);
        }

        public void InvokeOnGetGameState(GameStates gameState)
        {
            onGetGameStateSignal.Dispatch(gameState);
        }

        public void InvokeOnGameInit()
        {
            onGameInitSignal.Dispatch();
        }

        public void InvokeOnGameInitLevel()
        {
            onGameInitLevelSignal.Dispatch();
        }

        public void InvokeOnLevelInitialize()
        {
            onLevelInitializeSignal.Dispatch();
        }

        public void InvokeOnLevelIdleInitialize()
        {
            onLevelIdleInitializeSignal.Dispatch();
        }

        public void InvokeOnClearActiveIdleLevel()
        {
            onClearActiveIdleLevelSignal.Dispatch();
        }

        public void InvokeOnClearActiveLevel()
        {
            onClearActiveLevelSignal.Dispatch();
        }

        public void InvokeOnLevelFailed()
        {
            onLevelFailedSignal.Dispatch();
        }

        public void InvokeOnLevelSuccessful()
        {
            onLevelSuccessfulSignal.Dispatch();
        }

        public void InvokeOnNextLevel()
        {
            onNextLevelSignal.Dispatch();
        }

        public void InvokeOnRestartLevel()
        {
            onRestartLevelSignal.Dispatch();
        }

        public void InvokeOnPlay()
        {
            onPlaySignal.Dispatch();
        }

        public void InvokeOnReset()
        {
            onResetSignal.Dispatch();
        }

        public void InvokeOnStageAreaReached()
        {
            onStageAreaReachedSignal.Dispatch();
        }

        public void InvokeOnStageSuccessful()
        {
            onStageSuccessfulSignal.Dispatch();
        }

        public void InvokeOnGetLevelID(int levelID)
        {
            onGetLevelIDSignal.Dispatch(levelID);
        }

        public void Invoke