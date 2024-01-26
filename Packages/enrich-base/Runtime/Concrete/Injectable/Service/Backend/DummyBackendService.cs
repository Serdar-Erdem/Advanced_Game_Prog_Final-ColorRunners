using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend.Processor;
using Rich.Base.Runtime.Concrete.Data.ValueObject;
using Rich.Base.Runtime.Concrete.Promise;
using strange.extensions.context.api;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Service.Backend
{
    public class DummyBackendService
    {
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject RootObject { get; set; }

        private Dictionary<string, object> _dataMap;

        private List<string> _clearList;

        [UsedImplicitly]
        [PostConstruct]
        public void OnPostConstruct()
        {
            _clearList = new List<string>();
            _dataMap = new Dictionary<string, object>();
        }

        public void AddProcessor(IRequestProcessor processor)
        {
        }

        public IPromise Request(string command, string data)
        {
            Debug.Log("request " + command + "  " + data);
            Promise.Promise promise = new Promise.Promise();
            RequestInner(command);
            RootObject.GetComponent<MonoBehaviour>().StartCoroutine(ResolveIt(promise));
            return promise;
        }

        private IEnumerator ResolveIt(Promise.Promise promise)
        {
            yield return new WaitForSeconds(.1f);
            promise.Resolve();
        }

        public IPromise Request(string command, object data)
        {
            return Request(command, JsonConvert.SerializeObject(data));
        }

        public IPromise Request(string command)
        {
            return Request(command, "{}");
        }

        private void RequestInner(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                Debug.LogWarning("Extension cmd missing!");
                return;
            }

            _dataMap[command] = JsonConvert.SerializeObject(new GenericResponseVo());

            //Debug.Log("-------> Response " + command + " : " +
            //JsonConvert.SerializeObject((_dataMap[command])));


            // clear request
            //            _dataMap.Remove(command);
            if (_clearList.Contains(command))
            {
                _clearList.Remove(command);
            }
        }

        public void Send(string command, string data)
        {
            Debug.Log(command + " - " + data);
            //      var sfsObject = new SFSObject();
            //      sfsObject.PutUtfString(Sn, command);
            //      sfsObject.PutUtfString(Data, data);
            //
            //      _sfs.Send(new ExtensionRequest(command, sfsObject));
        }

        public void Listen(string command, ExtensionCallback callback)
        {
            //            ListenInner(command, callback);
        }

        public void RemoveListen(string command)
        {
            throw new NotImplementedException();
        }

        public T GetData<T>(string command) where T : new()
        {
            if (_dataMap.ContainsKey(command))
            {
                return JsonConvert.DeserializeObject<T>(_dataMap[command].ToString());
            }

            return new T();
        }

        public string GetData(string command)
        {
            if (_dataMap.ContainsKey(command))
            {
                return _dataMap[command].ToString();
            }

            return string.Empty;
        }

        public void Send(string command, object data)
        {
            Send(command, JsonConvert.SerializeObject(data));
        }

        public string ServerUrl { get; }
    }

}