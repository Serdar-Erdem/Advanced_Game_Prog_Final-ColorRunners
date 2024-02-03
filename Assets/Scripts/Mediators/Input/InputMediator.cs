using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Runtime.Model.Input;
using Runtime.Views.Input;
using Runtime.Signals;
using Runtime.Key;


namespace Runtime.Mediators.Input
{
    public class InputMediator : MediatorLite
    {
        [Inject] public InputSignals InputSignals { get; set; }
        [Inject] public IInputModel InputModel { get; set; }
        [Inject] public InputView InputView { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            InputView.onInputTaken += OnInputTaken;
            InputView.onInputDragged += OnInputDragged;
            InputView.onFirstTimeTouchTaken += OnFirstTimeTouchTaken;
            InputView.onInputReleased += OnInputReleased;


            InputSignals.onEnableInput += OnEnableInput;
            InputSignals.onDisableInput += OnDisableInput;
        }

        private void OnFirstTimeTouchTaken()
        {
            InputSignals.InvokeOnFirstTimeTouchTaken();
        }

        private void OnInputDragged(HorizontalInputParams inputParams)
        {
            InputSignals.InvokeOnInputDragged(inputParams);
        }

        private void OnInputReleased()
        {
            InputSignals.InvokeOnInputReleased();
        }

        private void OnInputTaken()
        {
            InputSignals.InvokeOnInputTaken();
        }

        public override void OnRemove()
        {
            base.OnRemove();
            InputView.onInputTaken -= OnInputTaken;
            InputView.onInputDragged -= OnInputDragged;
            InputView.onFirstTimeTouchTaken -= OnFirstTimeTouchTaken;
            InputView.onInputReleased -= OnInputReleased;


            InputSignals.onEnableInput -= OnEnableInput;
            InputSignals.onDisableInput -= OnDisableInput;
        }

        public override void OnEnabled()
        {
            InputView.SetInputData(InputModel.InputData.Data);
        }

        private void OnEnableInput()
        {
            InputView.EnableInput();
        }

        private void OnDisableInput()
        {
            InputView.DisableInput();
        }
    }
}
