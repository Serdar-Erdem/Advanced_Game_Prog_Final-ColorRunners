using UnityEngine;
using Data.ValueObjects;
using Controllers;
using Enums;
using System.Collections.Generic;
using Signals;

namespace Managers
{
    [Inject]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerMeshController meshController;
        [SerializeField] private PlayerTextController textController;
        [SerializeField] private PlayerAnimationController animationController;

        [SerializeField] private ColorTypes currentColor;

        private PlayerData data;
        private GameStates currentGameState = GameStates.Runner;

        public float CurrentScore { get; private set; }

        private void Awake()
        {
            data = GetPlayerData();
            SendPlayerDataToMovementController();
        }

        private void SendPlayerDataToMovementController()
        {
            movementController.SetMovementData(data.PlayerMovementData);
        }

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>("Data/CD_Player").PlayerData;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onIdleInputTaken += OnGetIdleInputValues;
            ScoreSignals.Instance.onUpdateScore += OnUpdateScoreText;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            PlayerSignal.Instance.onChangeVerticalSpeed += OnChangeVerticalSpeed;
            PlayerSignal.Instance.onIncreaseScale += OnIncreaseSize;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
            CoreGameSignals.Instance.onGetGameState += OnChangeGameState;
            InputSignals.Instance.onSidewaysEnable += OnSidewaysEnable;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onIdleInputTaken -= OnGetIdleInputValues;
            ScoreSignals.Instance.onUpdateScore -= OnUpdateScoreText;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            PlayerSignal.Instance.onChangeVerticalSpeed -= OnChangeVerticalSpeed;
            PlayerSignal.Instance.onIncreaseScale -= OnIncreaseSize;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
            CoreGameSignals.Instance.onGetGameState -= OnChangeGameState;
            InputSignals.Instance.onSidewaysEnable -= OnSidewaysEnable;
        }

        #endregion

        #region Subscribed Methods

        private void OnPlay()
        {
            movementController.EnableMovement();
        }

        private void OnLevelSuccessful()
        {
            movementController.IsReadyToPlay(false);
        }

        private void OnLevelFailed()
        {
            movementController.IsReadyToPlay(false);
        }

        public void StopVerticalMovement()
        {
            movementController.ChangeVerticalMovement(0);
        }

        public void ChangeAnimation(PlayerAnimationTypes animationType)
        {
            animationController.ChangeAnimation(animationType);
        }

        public void StopAllMovement()
        {
            movementController.StopAllMovement();
            ChangeAnimation(PlayerAnimationTypes.Idle);
        }

        public void EnableVerticalMovement()
        {
            movementController.ChangeVerticalMovement(10);
        }

        public void RepositionPlayerForDrone(GameObject other)
        {
            movementController.RepositionPlayerForDrone(other);
        }

        public void OnChangeVerticalSpeed(float verticalSpeed)
        {
            movementController.ChangeVerticalMovement(verticalSpeed);
        }

        public void OnSidewaysEnable(bool isSidewayEnable)
        {
            movementController.SetSidewayEnabled(isSidewayEnable);
        }

        private void OnReset()
        {
            movementController.OnReset();
        }

        private void OnUpdateScoreText(List<int> currentScores)
        {
            switch (currentGameState)
            {
                case GameStates.Idle:
                    textController.UpdatePlayerScore(currentScores[0]);
                    break;
                case GameStates.Runner:
                    textController.UpdatePlayerScore(currentScores[1]);
                    break;
                case GameStates.Failed:
                    CloseScoreText(true);
                    StopAllMovement();
                    break;
            }
        }

        public void OnIdleMovement()
        {
            // Do something related to idle movement if needed.
        }

        public void CloseScoreText(bool visibilityState)
        {
            textController.CloseScoreText(visibilityState);
        }

        #endregion

        public void OnIncreaseSize()
        {
            meshController.IncreasePlayerSize();
        }

        public void ActivateMesh()
        {
            NewCameraSignals.Instance.onChangeCameraState.Invoke(CameraStates.StartOfIdle);
            meshController.ActiveMesh();
        }

#endregion
    }
}
