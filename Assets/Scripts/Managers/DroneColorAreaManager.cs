using System.IO;
using Enums;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class DroneColorAreaManager : MonoBehaviour
    {
        [Inject] private bool openDroneAreaExit { get; set; }
        [Inject] private DroneAreaMeshController droneAreaMeshController { get; set; }
        [Inject] private MeshRenderer _meshRenderer { get; set; }

        public ColorTypes CurrentColorType;
        public MatchType matchType = MatchType.UnMatched;

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onGameInit += OnSetColor;
        }

        private void OnSetColor()
        {
            droneAreaMeshController.ChangeAreaColor(CurrentColorType);
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onGameInit -= OnSetColor;
        }
    }
}
