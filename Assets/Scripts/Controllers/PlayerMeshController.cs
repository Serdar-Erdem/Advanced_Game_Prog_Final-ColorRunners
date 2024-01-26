using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Controllers
{
    public class PlayerMeshController : View
    {
        [Inject]
        public IPlayerMeshService PlayerMeshService { get; set; }

        [SerializeField] private SkinnedMeshRenderer meshRenderer;

        public void IncreasePlayerSize()
        {
            if (CanIncreaseSize())
            {
                PlayerMeshService.IncreasePlayerSize(transform.parent);
            }
        }

        public void ActivateMesh()
        {
            PlayerMeshService.ActivateMesh(meshRenderer);
        }

        private bool CanIncreaseSize()
        {
            return transform.parent.localScale.x <= 3;
        }
    }

    public interface IPlayerMeshService
    {
        void ActivateMesh(SkinnedMeshRenderer meshRenderer);
    }
}
