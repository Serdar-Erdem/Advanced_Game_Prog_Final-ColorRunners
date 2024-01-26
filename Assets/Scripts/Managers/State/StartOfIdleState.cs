using Cinemachine;
using UnityEngine;
using strange.extensions.signal.impl;

namespace Managers.Abstracts.Concretes
{
    public class StartOfIdleState : CameraBaseState
    {
        [Inject]
        public CameraState camManager { get; set; }

        [Inject]
        public CinemachineVirtualCamera virtualCamera { get; set; }

        [Inject]
        public Transform followTarget { get; set; }

        public override async void EnterState()
        {
            await Task.Delay(1000);
        }

        public override void UpdateState()
        {
        }

        public override void OnCollisionEnter()
        {
        }
    }
}
