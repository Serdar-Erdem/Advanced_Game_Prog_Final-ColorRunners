using System;
using Enums;
using UnityEngine;
using Managers;
using Signals;
using strange.extensions.signal.impl;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [Inject]
        public IPlayerPhysicsService PlayerPhysicsService { get; set; }

        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private GameObject playerObj;

        private bool _enteredRoulette;

        private void OnTriggerEnter(Collider other)
        {
            PlayerPhysicsService.OnTriggerEnter(other, playerManager);
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerPhysicsService.OnTriggerExit(other, playerManager, ref _enteredRoulette);
        }
    }

    public interface IPlayerPhysicsService
    {
        void OnTriggerEnter(Collider other, PlayerManager playerManager);
        void OnTriggerExit(Collider other, PlayerManager playerManager, ref bool enteredRoulette);
    }

    public class PlayerPhysicsService : IPlayerPhysicsService
    {
        public void OnTriggerEnter(Collider other, PlayerManager playerManager)
        {
            if (other.CompareTag("DroneArea"))
                playerManager.CloseScoreText(true);
            else if (other.CompareTag("DroneAreaPhysics"))
            {
                playerManager.RepositionPlayerForDrone(other.gameObject);
                playerManager.EnableVerticalMovement();
            }
            else if (other.CompareTag("Market"))
                playerManager.ChangeAnimation(PlayerAnimationTypes.Throw);
            else if (other.CompareTag("Portal"))
                StackSignals.Instance.onColorChange?.Invoke(other.GetComponent<ColorController>().ColorType);
        }

        public void OnTriggerExit(Collider other, PlayerManager playerManager, ref bool enteredRoulette)
        {
            if (other.CompareTag("DroneArea"))
                playerManager.StopVerticalMovement();
            else if (other.CompareTag("Roulette") && !enteredRoulette)
            {
                ScoreSignals.Instance.onAddLevelTototalScore?.Invoke();
                playerManager.StopAllMovement();
                playerManager.ActivateMesh();
                enteredRoulette = true;
            }
        }
    }
}
