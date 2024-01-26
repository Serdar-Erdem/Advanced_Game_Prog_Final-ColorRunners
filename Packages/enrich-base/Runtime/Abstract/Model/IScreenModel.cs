using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Key;

namespace Rich.Base.Runtime.Abstract.Model
{
    public interface IScreenModel
    {
        /// <summary>
        /// Panels list on history
        /// </summary>
        List<IPanelVo> History { get; set; }
        
        List<IPanelVo> CurrentPanels {get;set;}
    }
}