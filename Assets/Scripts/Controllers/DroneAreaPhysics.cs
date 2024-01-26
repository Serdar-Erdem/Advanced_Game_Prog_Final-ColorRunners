using Enums;
using UnityEngine;
using strange.extensions.mediation.impl;
using System.IO;

namespace Controllers
{
    public class DroneAreaPhysics : View
    {
        [Inject]
        public DroneColorAreaManager DroneColorAreaManager { get; set; }

        [SerializeField] private Collider collider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collected"))
            {
                CheckCollectedColorType(other.GetComponentInParent<CollectableManager>());
            }
        }

        private void CheckCollectedColorType(CollectableManager collectableManager)
        {
            if (collectableManager.CurrentColorType == DroneColorAreaManager.CurrentColorType)
            {
                DroneColorAreaManager.MatchType = MatchType.Match;
            }
        }
    }
}
