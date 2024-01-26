using UnityEngine;
using Data.ValueObjects;
using Signals;
using UnityEngine.InputSystem;

using Enums;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [Inject] private InputData Data;

        private bool isFirstTimeTouchTaken = false;
        private bool isReadyForTouch;
        private Vector3 _moveVector;
        private Vector3 _playerMovementValue;
        private PlayerInputSystem _playerInput;

        private void Awake()
        {
        }

        private void InitialSettings()
        {
            _playerInput = new PlayerInputSystem();
            _playerMovementValue = Vector3.zero;
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            _playerInput.Runner.Enable();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onGetGameState += OnChangeInputType;

            _playerInput.Runner.MouseDelta.performed += OnPlayerInputMouseDeltaPerformed;
            _playerInput.Runner.MouseDelta.canceled += OnPlayerInputMouseDeltaCanceled;
            _playerInput.Runner.MouseLeftButton.started += OnMouseLeftButtonStart;

            _playerInput.Idle.JoyStick.performed += OnPlayerInputJoyStickPerformed;
            _playerInput.Idle.JoyStick.canceled += OnPlayerInputJoyStickCanceled;
            _playerInput.Idle.JoyStick.started += OnPlayerInputJoyStickStart;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;

            _playerInput.Runner.MouseDelta.performed -= OnPlayerInputMouseDeltaPerformed;
            _playerInput.Runner.MouseDelta.canceled -= OnPlayerInputMouseDeltaCanceled;
            _playerInput.Runner.MouseLeftButton.started -= OnMouseLeftButtonStart;

            _playerInput.Idle.JoyStick.performed -= OnPlayerInputJoyStickPerformed;
            _playerInput.Idle.JoyStick.canceled -= OnPlayerInputJoyStickCanceled;
            _playerInput.Idle.JoyStick.started -= OnPlayerInputJoyStickStart;
        }

        private void OnDisable()
        {
            _playerInput.Runner.Disable();
            UnSubscribeEvents();
        }

        #endregion

        #region Mouse Drag Methods

        private void OnPlayerInputMouseDeltaPerformed(InputAction.CallbackContext context)
        {
            InputSignals.Instance.onSidewaysEnable?.Invoke(true);

            _playerMovementValue = new Vector3(context.ReadValue<Vector2>().x, 0f, 0f);
            float horizontalInputSpeed = Mathf.Sign(_playerMovementValue.x) * Mathf.Clamp(Mathf.Abs(_playerMovementValue.x), 0, Data.HorizontalInputSpeed);
            _moveVector.x = Mathf.SmoothDamp(_moveVector.x, horizontalInputSpeed, ref _moveVector.x, Data.ClampSpeed);

            InputSignals.Instance.onInputDragged?.Invoke(new RunnerHorizontalInputParams()
            {
                XValue = _moveVector.x,
                ClampValues = new Vector2(Data.ClampSides.x, Data.ClampSides.y)
            });
        }

        private void OnPlayerInputMouseDeltaCanceled(InputAction.CallbackContext context)
        {
            InputSignals.Instance.onSidewaysEnable?.Invoke(false);
            _playerMovementValue = Vector3.zero;
        }

        private void OnMouseLeftButtonStart(InputAction.CallbackContext cntx)
        {
            if (!isFirstTimeTouchTaken)
            {
                isFirstTimeTouchTaken = true;
                CoreGameSignals.Instance.onPlay?.Invoke();
            }
        }

        #endregion

        #region JoyStick Methods

        private void OnPlayerInputJoyStickStart(InputAction.CallbackContext context)
        {
            _playerMovementValue = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
            InputSignals.Instance.onIdleInputTaken?.Invoke(new IdleInputParams()
            {
                IdleXValue = _playerMovementValue.x * Data.IdleInputSpeed,
                IdleZValue = _playerMovementValue.z * Data.IdleInputSpeed
            });
        }

        private void OnPlayerInputJoyStickPerformed(InputAction.CallbackContext context)
        {
            _playerMovementValue = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
            InputSignals.Instance.onIdleInputTaken?.Invoke(new IdleInputParams()
            {
                IdleXValue = _playerMovementValue.x * Data.IdleInputSpeed,
                IdleZValue = _playerMovementValue.z * Data.IdleInputSpeed
            });
        }

        private void OnPlayerInputJoyStickCanceled
(InputAction.CallbackContext context)
        {
        }

        #endregion


        private void OnChangeInputType(GameStates newState)
        {
