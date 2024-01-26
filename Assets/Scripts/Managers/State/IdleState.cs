using Cinemachine;
using UnityEngine;
using strange.extensions.signal.impl;

namespace Managers.Abstracts.Concretes
{
    public class IdleState : CameraBaseState
    {
        [Inject]
        public CameraState camManager { get; set; }

        [Inject]
        public CinemachineVirtualCamera virtualCamera { get; set; }

        [Inject]
        public Transform followTarget { get; set; }

        public override void EnterState()
        {
        }

        public override void UpdateState()
        {
        }

        public override void OnCollisionEnter()
        {
        }
    }
}
