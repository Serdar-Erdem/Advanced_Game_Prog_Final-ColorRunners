using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using strange.extensions.signal.impl;
using Enums;
using Managers;

namespace Controllers
{
    public class TurretAreaController : MonoBehaviour
    {
        [Inject]
        public ITurretAreaService TurretAreaService { get; set; }

        public ColorTypes ColorType;
        public Vector3 CurrentTargetPos;

        [SerializeField] private int turretSearchPeriod;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private TurretAreaManager turretAreaManager;
        [SerializeField] private Transform turret;

        private void Awake()
        {
            TurretAreaService.ChangeInitColor(ColorType, meshRenderer);
        }

        private void Start()
        {
            InvokeRepeating(nameof(TurretAreaService.GetRandomSearchPoint), 0f, turretSearchPeriod);
        }

        public void StartSearchRotation()
        {
            TurretAreaService.RotateToTargetPos(turret, CurrentTargetPos);
        }

        public void StartWarnedRotation(GameObject target)
        {
            CurrentTargetPos = target.transform.position;
            TurretAreaService.RotateToTargetPos(turret, CurrentTargetPos);
        }

        public void FireTurretAnimation()
        {
        }
    }

    public interface ITurretAreaService
    {
        void ChangeInitColor(ColorTypes colorType, MeshRenderer meshRenderer);
        void GetRandomSearchPoint();
        void ResetTurretArea();
        void RotateToTargetPos(Transform turret, Vector3 targetPos);
    }

    public class TurretAreaService : ITurretAreaService
    {
        public void ChangeInitColor(ColorTypes colorType, MeshRenderer meshRenderer)
        {
            var colorHandler = Addressables.LoadAssetAsync<Material>($"CoreColor/Color_{colorType}");
            meshRenderer.material = (colorHandler.WaitForCompletion() != null) ? colorHandler.Result : null;
        }

        public void GetRandomSearchPoint()
        {
        }

        public void RotateToTargetPos(Transform turret, Vector3 targetPos)
        {
            Vector3 relativePos = targetPos - turret.position;
            Quaternion finalRotate = Quaternion.LookRotation(relativePos);
            turret.rotation = Quaternion.Lerp(turret.rotation, finalRotate, 0.1f);
        }
    }
}
