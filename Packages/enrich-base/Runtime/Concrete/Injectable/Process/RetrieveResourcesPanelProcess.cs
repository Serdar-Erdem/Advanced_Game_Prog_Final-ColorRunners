using System.Collections;
using Rich.Base.Runtime.Abstract.Data.ValueObject;
using Rich.Base.Runtime.Abstract.Injectable.Process;
using Rich.Base.Runtime.Signals;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Process
{
    public class RetrieveResourcesPanelProcess : CoroutineProcess<RetrieveResourcesPanelProcess>
    {
        [Inject] public CoreScreenSignals CoreScreenSignals { get; set; }

        public IPanelVo PanelVo;

        protected override IEnumerator Routine()
        {
            ResourceRequest request = Resources.LoadAsync("Screens/" + PanelVo.Key, typeof(GameObject));
            yield return request;
            if (request.asset == null)
            {
                Debug.LogError("LoadFromResources! Panel not found!! " + PanelVo.Key);
                yield break;
            }

            CoreScreenSignals.AfterRetrievedPanel.Dispatch(PanelVo, request.asset as GameObject);
        }
    }
}