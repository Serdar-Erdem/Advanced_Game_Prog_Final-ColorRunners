using Rich.Base.Runtime.Abstract.View;
using UnityEngine;

namespace Modules.SRDebuggerAndCamera.View
{
    public class CinemachineView : RichView
    {
        public Transform freeCamParent;
        public Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
    }
}