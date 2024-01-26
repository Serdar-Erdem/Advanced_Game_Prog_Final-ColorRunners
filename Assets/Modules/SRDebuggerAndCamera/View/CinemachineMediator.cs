using Cinemachine;
using Modules.SRDebuggerAndCamera.Model;
using Modules.SRDebuggerAndCamera.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Animations;

namespace Modules.SRDebuggerAndCamera.View
{
    public class CinemachineMediator : Mediator
    {
        [Inject] public CinemachineView view { get; set; }

        [Inject] public BaseSrSignals BaseSr { get; set; }
        [Inject] public SRoptionsProject SRoptionsProject { get; set; }

        [Inject] public ProjectSRSignals ProjectSr { get; set; }

        private Vector3 _startPosition;
        private float _startFov;
        private Vector3 _startDirection;
        private CinemachineVirtualCamera _freeCam;
        
        //Use this for initialization.
        public override void OnRegister()
        {
            //Base
            base.OnRegister();
            BaseSr.Reset.AddListener(Reset);
            BaseSr.UpdateCam.AddListener(UpdateFreeCamRotation);
            BaseSr.UpdateFov.AddListener(UpdateFreeCamFov);
            BaseSr.UpdateZoom.AddListener(UpdateFreeCamZoom);
            BaseSr.FreeCam.AddListener(FreeCam);
            BaseSr.MainCam.AddListener(MainCam);
            CinemachineVirtualCameraBase[] childCameras = GetComponent<CinemachineStateDrivenCamera>().ChildCameras;
            for (int i = 0; i < childCameras.Length; i++)
            {
                if (childCameras[i].name == "FreeCam")
                {
                    _freeCam = (CinemachineVirtualCamera) childCameras[i];
                    _startPosition = _freeCam.GetComponent<ParentConstraint>().GetTranslationOffset(0);
                    break;
                }
            }

            _startDirection = (_freeCam.transform.position - _freeCam.LookAt.position);
            _startFov = _freeCam.GetComponent<CinemachineFollowZoom>().m_MinFOV;
            Reset();
        
            //Project

            //Registering to view;
            // enemyView.onOneFrameAfterStart += OnViewSendEvent;
        }

        public override void OnRemove()
        {
            //Base
            base.OnRemove();
            BaseSr.Reset.RemoveListener(Reset);
            BaseSr.UpdateCam.RemoveListener(UpdateFreeCamRotation);
            BaseSr.UpdateFov.RemoveListener(UpdateFreeCamFov);
            BaseSr.UpdateZoom.RemoveListener(UpdateFreeCamZoom);
            BaseSr.FreeCam.RemoveListener(FreeCam);
            BaseSr.MainCam.RemoveListener(MainCam);
            //Project
        
            //Always remember to unregister for a good practice.
            // enemyView.onOneFrameAfterStart -= OnViewSendEvent;
        }

        public void Reset()
        {
            _freeCam.GetComponent<ParentConstraint>().locked = false;
            view.freeCamParent.transform.localPosition = Vector3.zero;
            _freeCam.GetComponent<ParentConstraint>().locked = true;
            _freeCam.transform.localPosition = _startPosition;
            SRoptionsProject.Fov = _startFov;
            SRoptionsProject.Zoom = 0;
            SRoptionsProject.CameraH = 0;
            SRoptionsProject.CameraV = 0;
           
        }

        public void MainCam()
        {
            view.animator.SetBool("DoMain", true);
        }

        public void FreeCam()
        {
            view.animator.SetBool("DoFreeCam", true);
        }

        public void UpdateFreeCamRotation(Vector2 eulerAngles)
        {
           
            // CinemachineVirtualCameraBase[] childCameras = GetComponent<CinemachineStateDrivenCamera>().ChildCameras;
            view.freeCamParent.transform.localEulerAngles = eulerAngles;
        }

        public void UpdateFreeCamFov(float fov)
        {    
            CinemachineVirtualCameraBase[] childCameras = GetComponent<CinemachineStateDrivenCamera>().ChildCameras;
            _freeCam.GetComponent<CinemachineFollowZoom>().m_MinFOV = fov;
        }
        public void UpdateFreeCamZoom(float Zoom)
        {    
            Vector3 currentOffset = _freeCam.GetComponent<ParentConstraint>().GetTranslationOffset(0);
            _freeCam.GetComponent<ParentConstraint>().SetTranslationOffset( 0,new Vector3(currentOffset.x,currentOffset.y, (_startPosition.z - Zoom*_startPosition.z/100)));
        }
    }
}