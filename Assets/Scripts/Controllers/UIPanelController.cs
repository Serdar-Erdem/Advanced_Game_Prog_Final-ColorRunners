using System;
using System.Collections.Generic;
using Enums;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Controllers
{
    public class UIPanelController : MonoBehaviour
    {
        [Inject]
        public IUIPanelService UIPanelService { get; set; }

        public void TogglePanel(UIPanelTypes panelParam, bool state)
        {
            UIPanelService.TogglePanel(panelParam, state);
        }

        public void OpenPanel(UIPanelTypes panelParam)
        {
            TogglePanel(panelParam, true);
        }

        public void ClosePanel(UIPanelTypes panelParam)
        {
            TogglePanel(panelParam, false);
        }
    }

    public class IUIPanelService
    {
        internal void TogglePanel(UIPanelTypes panelParam, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
