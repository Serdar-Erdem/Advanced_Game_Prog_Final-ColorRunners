using Enums;
using strange.extensions.command.impl;
using System;
using System.Windows.Input;
using UnityEngine;

namespace Controllers
{
    public class CollectableAnimationController : MonoBehaviour, ICommand
    {
        [Inject]
        public Animator Animator { get; set; }

        [Inject]
        public CollectableAnimationTypes AnimationType { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            PlayAnimation();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        private void PlayAnimation()
        {
            if (Animator != null)
            {
                Animator.SetTrigger(AnimationType.ToString());
            }
            else
            {
                Debug.LogError("Animator is not assigned in CollectableAnimationController.");
            }
        }
    }
}
