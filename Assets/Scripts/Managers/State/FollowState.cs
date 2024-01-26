using Cinemachine;
using UnityEngine;
using strange.extensions.signal.impl;

namespace Managers.Abstracts.Concretes
{
    public class FollowState : CameraBaseState
    {
        [Inject]
        public CameraState camManager { get; set; }

        [Inject]
        public CinemachineVirtualCamera virtualCamera { get; set; }

        [Inject]
        public Transform followTarget { get; set; }

        public override void EnterState()
        {
            camManager.StartCoroutine(ChangeFollowTargetAsync(virtualCamera, followTarget));
        }

        private System.Collections.IEnumerator ChangeFollowTargetAsync(CinemachineVirtualCamera virtualCamera, Transform followTarget)
        {
            yield return new WaitForSeconds(1f);
            virtualCamera.Follow = followTarget;
        }

        public override void UpdateState()
        {
        }

        public override void OnCollisionEnter()
        {
        }
    }
}
