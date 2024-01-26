using Rich.Base.Runtime.Abstract.Model;
using strange.extensions.mediation.impl;

namespace Rich.Base.Runtime.Concrete.View
{
    /// <summary>
    /// If pool access needed in any ...View script. This view should be derived from this script.
    /// </summary>
    public class PoolView : EventView
    {
        /// <summary>
        /// Pool model
        /// </summary>
        [Inject] public IObjectPoolModel pool { get; set; }
    }
}

