using UnityEngine;
using strange.extensions.signal.impl;

namespace Controllers
{
    public class TurretAreaPhysicController : MonoBehaviour
    {
        [Inject]
        public ITurretAreaService TurretAreaService { get; set; }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Collected"))
            {
                TurretAreaService.ResetTurretArea();
            }
        }
    }
}
