using Enums;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Controllers
{
    public class PlayerAnimationController : View
    {
        [Inject]
        public IPlayerAnimationService PlayerAnimationService { get; set; }

        [SerializeField] private Animator animator;

        public void ChangeAnimation(PlayerAnimationTypes animationType)
        {
            if (animationType == PlayerAnimationTypes.Run)
            {
                PlayRunningAnimation();
            }
            else
            {
                SetAnimationTrigger(animationType);
            }
        }

        private void PlayRunningAnimation()
        {
            PlayerAnimationService.PlayRunningAnimation(animator);
        }

        private void SetAnimationTrigger(PlayerAnimationTypes animationType)
        {
            PlayerAnimationService.SetAnimationTrigger(animator, animationType.ToString());
        }
    }

    public interface IPlayerAnimationService
    {
    }
}
