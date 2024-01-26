using System.Threading.Tasks;
using DG.Tweening;
using Managers;
using strange.extensions.mediation.impl;
using UnityEngine;
using Data.ValueObjects;
using Enums;
using Keys;

namespace Controllers
{
    public class PlayerMovementController : View
    {
        [Inject]
        public IPlayerMovementService PlayerMovementService { get; set; }

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Transform playerMesh;

        private PlayerMovementData _movementData;
        private bool _isReadyToMove, _isReadyToPlay;
        private float _inputValue, _inputValueX, _inputValueZ;
        private Vector2 _clampValues;
        private bool _sidewaysEnable;

        public GameStates CurrentGameState = GameStates.Runner;

        [Inject]
        public IPlayerMovementService PlayerMovementService { get; set; }

        public void SetMovementData(PlayerMovementData playerMovementData) => _movementData = playerMovementData;

        public void EnableMovement()
        {
            _isReadyToMove = true;
            _isReadyToPlay = true;
        }

        public void DisableMovement() => _isReadyToMove = false;

        public void UpdateRunnerInputValue(RunnerHorizontalInputParams inputParam)
        {
            _inputValue = inputParam.XValue;
            _clampValues = inputParam.ClampValues;
        }

        public void UpdateIdleInputValue(IdleInputParams inputParam)
        {
            _inputValueX = inputParam.IdleXValue;
            _inputValueZ = inputParam.IdleZValue;
        }

        public void IsReadyToPlay(bool state) => _isReadyToPlay = state;

        private void FixedUpdate()
        {
            if (_isReadyToPlay && _isReadyToMove && _sidewaysEnable)
                PlayerMovementService.Move(CurrentGameState, _inputValue, _inputValueX, _inputValueZ, _clampValues, _movementData, rigidbody, playerManager, playerMesh);
            else
                PlayerMovementService.StopSideways(rigidbody, _movementData);
        }

        public void Stop()
        {
            PlayerMovementService.Stop(rigidbody);
        }

        public void OnReset()
        {
            Stop();
            _isReadyToPlay = false;
            _isReadyToMove = false;
        }

        public void SetSidewayEnabled(bool isSidewayEnabled) => _sidewaysEnable = isSidewayEnabled;

        public void ChangeVerticalMovement(float verticalSpeed) => _movementData.ForwardSpeed = verticalSpeed;

        public async void RepositionPlayerForDrone(GameObject other)
        {
            await Task.Delay(200);
            PlayerMovementService.RepositionPlayerForDrone(transform, other.transform, playerManager);
        }

        public void DisableStopVerticalMovement()
        {
            _movementData.ForwardSpeed = 0;
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void StopAllMovement() => _isReadyToPlay = false;

        public void EnableIdleMovement()
        {
            _isReadyToPlay = true;
            _sidewaysEnable = true;
        }
    }

    public interface IPlayerMovementService
    {
    }
}
