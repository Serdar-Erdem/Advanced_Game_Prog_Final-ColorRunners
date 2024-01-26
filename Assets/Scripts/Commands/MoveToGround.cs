using Commands.Signals;
using DG.Tweening;
using Enums;
using Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Commands
{
    public class MoveToGroundCommand : Command
    {
        private object transform;

        [Inject]
        public Transform GroundTransform { get; set; }

        [Inject]
        public CollectableManager CollectableManager { get; set; }

        [Inject]
        public CoreGameSignals CoreGameSignals { get; set; }

        public override void Execute()
        {
            float zValue = Random.Range(-(GroundTransform.localScale.z / 3), (GroundTransform.localScale.z / 3));

            transform.DOMove(new Vector3(GroundTransform.position.x, transform.position.y, GroundTransform.position.z + zValue), 2f)
                .OnComplete(() =>
                {
                    if (CollectableManager != null)
                    {
                        CollectableManager.ChangeAnimationOnController(CollectableAnimationTypes.Crouch);
                    }
                });

            CoreGameSignals.onMoveToGroundComplete?.Invoke();
        }
    }
}
