using Enums;
using UnityEngine;
using strange.extensions.mediation.impl;
using Managers;
using System.IO;

namespace Controllers
{
    public class CollectablePhysicsController : View
    {
        [Inject]
        public CollectableManager Manager { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (CompareTag("Collected"))
            {
                HandleCollectedTrigger(other);
            }

            if (other.CompareTag("Roulette"))
            {
                Manager.DecreaseStackOnIdle();
            }

            if (other.CompareTag("DroneAreaPhysics"))
            {
                HandleDroneAreaPhysicsTrigger(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (CompareTag("Collected"))
            {
                if (other.CompareTag("TurretAreaGround"))
                {
                    gameObject.tag = "Collected";
                    Manager.ChangeAnimationOnController(CollectableAnimationTypes.Run);
                }

                if (other.CompareTag("DroneAreaPhysics"))
                {
                    Manager.ChangeAnimationOnController(CollectableAnimationTypes.Run);
                }
            }
        }

        private void HandleCollectedTrigger(Collider other)
        {
            if (other.CompareTag("Collectable"))
            {
                HandleCollectableCollision(other);
            }

            if (other.CompareTag("Obstacle"))
            {
                Destroy(other.transform.parent.gameObject);
                Manager.DecreaseStack();
            }

            if (other.CompareTag("TurretAreaGround"))
            {
                HandleTurretAreaCollision(other);
            }

            if (other.CompareTag("ColoredGround"))
            {
                HandleColoredGroundCollision(other);
            }
        }

        private void HandleCollectableCollision(Collider other)
        {
            CollectableManager otherCollectableManager = other.transform.parent.GetComponent<CollectableManager>();

            if (otherCollectableManager.CurrentColorType == Manager.CurrentColorType)
            {
                other.transform.tag = "Collected";
                otherCollectableManager.IncreaseStack();
            }
            else
            {
                Destroy(other.transform.parent.gameObject);
                Manager.DecreaseStack();
            }
        }

        private void HandleTurretAreaCollision(Collider other)
        {
            TurretAreaController turretAreaController = other.GetComponent<TurretAreaController>();
            TurretAreaManager turretAreaManager = other.GetComponentInParent<TurretAreaManager>();

            Manager.ChangeAnimationOnController(CollectableAnimationTypes.CrouchRun);

            if (Manager.CurrentColorType != turretAreaController.ColorType)
            {
                turretAreaManager.AddTargetToList(transform.parent.gameObject);
            }
        }

        private void HandleColoredGroundCollision(Collider other)
        {
            Manager.DeListStack();
            Manager.SetCollectablePositionOnDroneArea(other.gameObject.transform);
            Manager.CheckColorType(other.GetComponent<DroneColorAreaManager>());
            tag = "Untagged";
        }

        private void HandleDroneAreaPhysicsTrigger(Collider other)
        {
            tag = "Collected";

            if (Manager.MatchType == MatchType.Match)
            {
                Manager.ChangeOutlineState(true);
                Manager.IncreaseStack();
            }
            else
            {
                Manager.DelayedDeath(true);
            }
        }
    }
}
