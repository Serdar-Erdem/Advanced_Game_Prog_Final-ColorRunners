using System;
using Enums;
using UnityEngine;
using DG.Tweening;
using Signals;
using strange.extensions.signal.impl;
using Cinemachine;
using Managers.Abstracts.Concretes;
using Managers.Abstracts;

namespace Managers
{
    public class CameraState : MonoBehaviour
    {
        [Inject]
        public Transform initPosition { get; set; }

        [Inject]
        public CinemachineVirtualCamera virtualCamera { get; set; }

        private Vector3 initialPosition;
        private Transform playerManager;

        [Inject]
        public CameraBaseState currentState { get; set; }

        [Inject]
        public StartState startState { get; set; }

        [Inject]
        public RunnerState runnerState { get; set; }

        [Inject]
        public IdleState idleState { get; set; }

        [Inject]
        public StartOfIdleState startOfIdleState { get; set; }

        private void Awake()
        {
            GetInitialPosition();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void Start()
        {
            OnCameraInitialization();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnCameraInitialization;
            CameraSignals.Instance.onChangeCameraStates += OnChangeCameraState;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnCameraInitialization;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void GetInitialPosition()
        {
            initialPosition = transform.localPosition;
        }

        private void OnMoveToInitialPosition()
        {
            transform.localPosition = initialPosition;
        }

        private void OnCameraInitialization()
        {
            ChangeState(startState);
        }

        private void OnChangeCameraState(CameraStates _currentState)
        {
            switch (_currentState)
            {
                case CameraStates.StartState:
                    ChangeState(startState);
                    break;

                case CameraStates.Runner:
                    ChangeState(runnerState);
                    break;

                case CameraStates.Idle:
                    ChangeState(idleState);
                    break;

                case CameraStates.StartOfIdle:
                    ChangeState(startOfIdleState);
                    break;
            }
        }

        private void ChangeState(CameraBaseState _cameraState)
        {
            playerManager = GameObject.FindWithTag("Player").transform;
            currentState = _cameraState;
            currentState.EnterState(this, virtualCamera, playerManager);
        }

        private void OnReset()
        {
            virtualCamera.Follow = null;
            virtualCamera.LookAt = null;
            OnMoveToInitialPosition();
        }
    }
}
