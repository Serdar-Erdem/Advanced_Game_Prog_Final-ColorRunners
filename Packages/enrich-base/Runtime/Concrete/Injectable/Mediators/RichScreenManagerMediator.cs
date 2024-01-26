using System;
using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Function;
using Rich.Base.Runtime.Abstract.Key;
using Rich.Base.Runtime.Abstract.Model;
using Rich.Base.Runtime.Abstract.View;
using Rich.Base.Runtime.Concrete.View;
using Rich.Base.Runtime.Signals;
using strange.extensions.mediation.api;
using UnityEngine;

//Currently multi-screen manager is not supported, only have one of this in your app.
namespace Rich.Base.Runtime.Concrete.Injectable.Mediator
{
    public class RichScreenManagerMediator : strange.extensions.mediation.impl.Mediator, IMediator
    {
        [Inject] public RichScreenManagerView View { get; set; }

        [Inject] public IScreenModel ScreenModel { get; set; }

        [Inject] public CoreScreenSignals CoreScreenSignals{get;set;}

        private List<GameObject> _panels;
        /// <summary>
        /// Works after all bindings are completed. 
        /// Useful to attach listeners
        /// After Awake 
        /// Before Start. 
        /// </summary>
        public override void OnRegister()
        {
            CoreScreenSignals.AfterRetrievedPanel.AddListener(CreatePanel);
            CoreScreenSignals.DisplayPanel.AddListener(OnDisplayPanel);
            CoreScreenSignals.ClearLayerPanel.AddListener(OnClearLayer);
            CoreScreenSignals.GoBackScreen.AddListener(OnBack);

            _panels = new List<GameObject>();
            foreach (Transform layer in View.Layers)
            {
                foreach (Transform panel in layer)
                {
                    _panels.Add(panel.gameObject);
                }
            }
        
            Debug.Log(GetType() + " registered with context");
        }

        /// <summary>
        /// Remove the current page. Check the previous page and load it.
        /// </summary>
        private void OnBack()
        {
            if (ScreenModel.History.Count < 2)
                return;

            ScreenModel.History.RemoveAt(ScreenModel.History.Count - 1);
            IPanelVo prePanelVo = ScreenModel.History[ScreenModel.History.Count - 1];
            ScreenModel.History.RemoveAt(ScreenModel.History.Count - 1);

            //Creating signal argument
            CoreScreenSignals.DisplayPanel.Dispatch(prePanelVo);
        }

        /// <summary>
        /// Receives the display panel request
        /// </summary>
        private void OnDisplayPanel(IPanelVo panelVo)
        {
            if (panelVo.Key == null)
            {
                Debug.LogError("Panel is null");
                return;
            }
            Debug.Log("Displaying Panel " + panelVo.Key);

            RetrievePanel(panelVo);
        }

        /// <summary>
        /// Checks if the display panel request is valid and raises retrieve signal.
        /// </summary>
        private void RetrievePanel(IPanelVo panelVo)
        {
            if (panelVo.LayerIndex >= View.Layers.Length)
            {
                Debug.LogError("There is no layer " + panelVo.LayerIndex);
                return;
            }

            CoreScreenSignals.RetrievePanel.Dispatch(panelVo);
        }

        /// <summary>
        /// Remove the last screen added by name
        /// </summary>
        /// <param name="panelVo"></param>
        private void RemoveFromHistoryByNameFromLast(string name)
        {
            for (int i = ScreenModel.History.Count - 1; i > 0; i--)
            {
                if (name == ScreenModel.History[i].Name)
                {
                    ScreenModel.History.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Create the panel and set the transform of gameobject
        /// </summary>
        /// <param name="vo"> PanelVo which is stored on View objects, if it is a screen </param>
        /// <param name="template"> Prefab to create </param>
        private void CreatePanel(IPanelVo panelVo, GameObject template)
        {
            Debug.Log("Creating Panel " + panelVo.Key);
        
            if (panelVo.RemoveSamePanels)
                RemoveSamePanels(panelVo.Key,panelVo.LayerIndex);

            if (panelVo.RemoveLayer)
                RemoveLayer(panelVo.LayerIndex);

            if (panelVo.RemoveAll)
                RemoveAllPanels();
        
            //This can be a pool!
            GameObject newPanel = Instantiate(template,View.Layers[panelVo.LayerIndex]);
            IPanel panel = newPanel.GetComponent<IPanel>();
            RichView view = panel as RichView;
            if(view == null)
            {
                Debug.LogError("This is not a view!",newPanel);
                return;
            }
            Debug.Log("Setting Panel Vo");
            panel.vo = panelVo;
            view.Initialize();
        
            newPanel.transform.SetParent(View.Layers[panelVo.LayerIndex], false);
            newPanel.transform.localScale = Vector3.one;

            _panels.Add(newPanel);

            if (!panelVo.IgnoreHistory)
                ScreenModel.History.Add(panel.vo);

            ScreenModel.CurrentPanels.Add(panel.vo);
            //Debug.Log("---------------------" + vo.Type);
        }

        /// <summary>
        /// Used to prevent having same panels on a layer
        /// </summary>
        private void RemoveSamePanels(string key, int layerIndex)
        {
            foreach (Transform child in View.Layers[layerIndex].transform)
            {
                ScreenModel.CurrentPanels.RemoveAll(vo=>vo.Key == key);
                //if (App.Status.value.HasFlag(child.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= child.GetComponent<IPanelView>().vo.Type;

                int index = child.name.IndexOf(key, StringComparison.Ordinal);
                if (index != -1)
                {
                    Destroy(child.gameObject);
                    RemoveFromHistoryByNameFromLast(key);
                }
            }
        }

        /// <summary>
        /// Clear all the gameobjecs on the given layer
        /// </summary>
        private void OnClearLayer(int layer)
        {
            RemoveLayer(layer);
        }

        /// <summary>
        /// Clear all gameobjects on layer. Called when loading a new screen
        /// </summary>
        private void RemoveLayer(int voLayerIndex)
        {
            foreach (Transform panel in View.Layers[voLayerIndex].transform)
            {
                ScreenModel.CurrentPanels.Remove(panel.GetComponent<IPanel>().vo);
                //if (App.Status.value.HasFlag(panel.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= panel.GetComponent<IPanelView>().vo.Type;

                Destroy(panel.gameObject);
                _panels.Remove(panel.gameObject);
            }
        }

        /// <summary>
        /// Clear all panels on all layers
        /// </summary>
        private void RemoveAllPanels()
        {
            foreach (var panel in _panels)
            {
                ScreenModel.CurrentPanels.Remove(panel.GetComponent<IPanel>().vo);
                //if (App.Status.value.HasFlag(panel.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= panel.GetComponent<IPanelView>().vo.Type;

                Destroy(panel);
            }

            _panels.Clear();
        }

        /// <summary>
        /// Works when connected gameobject is destroyed. 
        /// Useful to remove listeners
        /// Before OnDestroy method
        /// </summary>
        public override void OnRemove()
        {
            CoreScreenSignals.AfterRetrievedPanel.RemoveListener(CreatePanel);
            CoreScreenSignals.DisplayPanel.RemoveListener(OnDisplayPanel);
            CoreScreenSignals.ClearLayerPanel.RemoveListener(OnClearLayer);
            CoreScreenSignals.GoBackScreen.RemoveListener(OnBack);
        }
    }
}