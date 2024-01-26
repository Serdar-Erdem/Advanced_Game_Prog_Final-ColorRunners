using System.Collections.Generic;
using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Key;
using Rich.Base.Runtime.Abstract.Model;
using Sirenix.OdinInspector;

namespace Rich.Base.Runtime.Concrete.Model
{
  
  
  public class ScreenModel : IScreenModel
  {
    [PostConstruct]
    public void OnPostConstruct()
    {
        CurrentPanels = new List<IPanelVo>();
        History = new List<IPanelVo>();
    }

    [ShowInInspector]
    [ListDrawerSettings(ShowIndexLabels = true,ListElementLabelName = "Name")]
    public List<IPanelVo> History { get; set; }

    [ShowInInspector]
    public List<IPanelVo> CurrentPanels { get; set; }

    public string GetHistoryData()
    {
      string data = string.Empty;
      foreach (IPanelVo panelVo in History)
      {
        data += panelVo.Name + ",";
      }

      return data;
    }
  }
}