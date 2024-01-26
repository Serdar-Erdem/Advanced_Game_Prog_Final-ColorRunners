using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;
using strange.extensions.mediation.impl;

namespace Controllers
{
    public class DroneAreaMeshController : View
    {
        [Inject]
        public DroneAreaManager DroneAreaManager { get; set; }

        [SerializeField] private MeshRenderer meshRenderer;

        public void ChangeAreaColor(ColorTypes colorType)
        {
            var colorHandler = Addressables.LoadAssetAsync<Material>($"CoreColor/Color_{colorType}");
            meshRenderer.material = colorHandler.Result;

            // DroneAreaManager'a renk deðiþikliðini bildirebilirsiniz.
            DroneAreaManager.OnAreaColorChanged(colorType);
        }
    }
}
