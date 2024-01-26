using Cinemachine;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Managers.Abstracts
{
    public abstract class CameraBaseState
    {
        [Inject]
        public CameraBaseState _camManager { get; set; }

        [Inject]
        public CinemachineVirtualCamera virtualCamera { get; set; }

        [Inject]
        public Transform followTarget { get; set; }

        public void EnterState(CameraBaseState camManager, CinemachineVirtualCamera camera, Transform target)
        {
            _camManager = camManager;
            virtualCamera = camera;
            followTarget = target;
            EnterState();
        }

        public abstract void UpdateState();
        public abstract void OnCollisionEnter();

        protected virtual void EnterState() { }
    }
}
