using System.Collections.Generic;
using Rich.Base.Runtime.Concrete.Injectable.Service.Backend;
using Rich.Base.Runtime.Concrete.Promise;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Rich.Base.Runtime.Abstract.Injectable.Service.Backend
{
    public interface IProcessorService
    {
        List<string> ClearList { get; }

        Dictionary<string, Promise> PromiseMap { get; }

        Dictionary<string, object> ResponseMap { get; }

        string ServerUrl { get; }

        string SessionId { get; set; }
        
        Dictionary<string, ExtensionCallback> ExtensionMap { get; }

        void SetResponse(string command, object data);

        void UpdateProgressBar(string key, long progress);
    }
}