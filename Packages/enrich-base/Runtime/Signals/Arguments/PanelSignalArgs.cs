using Rich.Base.Runtime.Abstract.Key;

namespace Rich.Base.Runtime.Signals.Arguments
{
    public struct PanelSignalArgs
    {
        /// <summary>
        /// Fill this in order to define the operated panel. 
        /// </summary>
        public IPanelKey Key;
        
        public int LayerIndex;
        
        public bool RemoveSamePanels;
        
        public bool RemoveLayer;
        
        public bool RemoveAll;
    }
}