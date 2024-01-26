namespace Rich.Base.Runtime.Signals
{
    [System.Serializable]
    public struct OpenNormalPanelArgs
    {
        public string PanelKey;
        public int LayerIndex;
        public bool IgnoreHistory;
    }
}