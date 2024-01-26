using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend.Listener;
using Rich.Base.Runtime.Constant;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Service.Backend.Listener
{
    /// <summary>
    /// Use this structure when you need to listen somethings during the all time since app starts
    /// </summary>
    public class BackendServiceListener : IBackendServiceListener
    {
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }

        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        [Inject] public IBackendService backendService { get; set; }

        private List<string> _processorList;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _processorList = new List<string>();
            var ServiceList = GetConstants(typeof(BackendServiceFunctions));

            foreach (FieldInfo info in ServiceList)
            {
                var item = info.GetValue(null) as string;

                if (item == null)
                    continue;
                var splitItems = item.Split('_');
                if (!splitItems[0].Equals("socketlisten"))
                {
                    continue;
                }

                _processorList.Add(item);
            }

            ListenExitingProcessors();
        }

        private void ListenExitingProcessors()
        {
            foreach (var key in _processorList)
            {
                backendService.Listen(key, delegate { OnListenCallBack(key); });
            }
        }

        private void OnListenCallBack(string key)
        {
            dispatcher.Dispatch(key);
        }

        private List<FieldInfo> GetConstants(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                                                    BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }
    }
}
