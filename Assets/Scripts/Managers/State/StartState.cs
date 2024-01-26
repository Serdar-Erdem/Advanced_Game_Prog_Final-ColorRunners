using Cinemachine;
using UnityEngine;
using Signals;
using Enums;

namespace Managers.Abstracts.Concretes
{
    public class MyStartState : CameraBaseState
    {
        [Inject]
        public CameraState camManager { get; set; }

        [Inject]
        public CinemachineVirtualCamera virtualCamera { get; set; }

        [Inject]
        public Transform followTarget { get; set; }

        public override async void EnterState()
        {
            Debug.Log("MyStartState EnterState is working");

            await Task.Delay(1000);

            virtualCamera.Follow = camManager.initPosition;
            CameraSignals.Instance.onChangeCameraStates?.Invoke(CameraStates.Runner);
        }

        public override void UpdateState()
        {
        }

        public override void OnCollisionEnter()
        {
        }
    }
}
