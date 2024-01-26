using System.Threading.Tasks;
using UnityEngine;
using Commands;
using Enums;
using Signals;
using System;
using Zenject;
using System.IO;

namespace Controllers
{
    public class CollectableManager : MonoBehaviour
    {
        [Inject] private MoveToGround movementCommand;
        [Inject] private CollectableMeshController collectableMeshController;
        [Inject] private CapsuleCollider collider;
        [Inject] private CollectablePhysicsController collectablePhysicsController;
        [Inject] private CollectableAnimationController collectableAnimationController;

        public ColorTypes CurrentColorType;
        public MatchType MatchType;

        [Inject]
        public void Construct(MoveToGround movementCommand, CollectableMeshController collectableMeshController,
                              CapsuleCollider collider, CollectablePhysicsController collectablePhysicsController,
                              CollectableAnimationController collectableAnimationController)
        {
            this.movementCommand = movementCommand;
            this.collectableMeshController = collectableMeshController;
            this.collider = collider;
            this.collectablePhysicsController = collectablePhysicsController;
            this.collectableAnimationController = collectableAnimationController;
        }

        private void Awake()
        {
            ChangeColor(CurrentColorType);
        }

        public void ChangeColor(ColorTypes colorType)
        {
            CurrentColorType = colorType;
            collectableMeshController.ChangeCollectableMaterial(CurrentColorType);
        }

        public void DecreaseStack()
        {
            HandleStackDecrease();
            DelayedDeath(false);
        }

        public void DecreaseStackOnIdle()
        {
            HandleStackDecrease();
            DelayedDeath(false);
            PlayerSignal.Instance.onIncreaseScale?.Invoke();
        }

        public void DeListStack()
        {
            StackSignals.Instance.onDroneArea?.Invoke(transform.GetSiblingIndex());
        }

        public void IncreaseStack()
        {
            StackSignals.Instance.onIncreaseStack?.Invoke(gameObject);
            collectableAnimationController.ChangeAnimation(CollectableAnimationTypes.Run);
        }

        public async void SetCollectablePositionOnDroneArea(Transform groundTransform)
        {
            collectableAnimationController.ChangeAnimation(CollectableAnimationTypes.Run);
            movementCommand.MoveToGround(groundTransform);
            collectableMeshController.ActivateOutline(false);
            await Task.Delay(3000);
        }

        public void ChangeOutlineState(bool state)
        {
            collectableMeshController.ActivateOutline(state);
        }

        public void DelayedDeath(bool isDelayed)
        {
            collectableAnimationController.ChangeAnimation(CollectableAnimationTypes.Death);
            ChangeOutlineState(true);
            Destroy(gameObject, isDelayed ? 3f : 0.1f);
        }

        public void CheckColorType(DroneColorAreaManager droneColorAreaManager)
        {
            collectableMeshController.CheckColorType(droneColorAreaManager);
        }

        private void OnDestroy()
        {
        }

        private void HandleStackDecrease()
        {
            StackSignals.Instance.onDecreaseStackRoullette?.Invoke(transform.GetSiblingIndex());
            gameObject.transform.parent = null;
        }

        internal void ChangeAnimationOnController(CollectableAnimationTypes crouch)
        {
            throw new NotImplementedException();
        }

        internal void CheckColorType(DroneColorAreaManager droneColorAreaManager)
        {
            throw new NotImplementedException();
        }
    }
}
