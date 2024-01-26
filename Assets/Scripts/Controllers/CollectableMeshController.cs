using System.Threading.Tasks;
using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;
using strange.extensions.mediation.impl;
using System.IO;

namespace Controllers
{
    public class CollectableMeshController : View
    {
        [Inject]
        public CollectableManager CollectableManager { get; set; }

        [SerializeField]
        private SkinnedMeshRenderer meshRenderer;

        public void ChangeCollectableMaterial(ColorTypes colorType)
        {
            var colorHandler = Addressables.LoadAssetAsync<Material>($"Collectable/Color_{colorType}");
            meshRenderer.material = colorHandler.WaitForCompletion() != null ? colorHandler.Result : null;
        }

        public void CheckColorType(DroneColorAreaManager droneColorAreaRef)
        {
            CollectableManager.MatchType = (CollectableManager.CurrentColorType == droneColorAreaRef.CurrentColorType)
                ? MatchType.Match
                : MatchType.UnMatched;
        }

        public async void ActivateOutline(bool isOutlineActive)
        {
            float outlineValue = isOutlineActive ? 70 : 0;
            await Task.Delay(2000);
            meshRenderer.material.DOFloat(outlineValue, "_OutlineSize", 1f);
        }
    }
}
