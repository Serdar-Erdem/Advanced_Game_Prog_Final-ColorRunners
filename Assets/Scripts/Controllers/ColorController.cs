using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;
using strange.extensions.command.impl;
using System.Windows.Input;
using System;

namespace Controllers
{
    public class ColorController : MonoBehaviour, ICommand
    {
        [Inject]
        public ColorTypes ColorType { get; set; }

        [SerializeField]
        private MeshRenderer meshRenderer;

        public event EventHandler CanExecuteChanged;

        public void Execute()
        {
            ChangeAreaColor(ColorType);
        }

        private void ChangeAreaColor(ColorTypes colorType)
        {
            var colorHandler = Addressables.LoadAssetAsync<Material>($"PortalColors/Color_{colorType}");
            meshRenderer.material = colorHandler.WaitForCompletion() != null ? colorHandler.Result : null;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
