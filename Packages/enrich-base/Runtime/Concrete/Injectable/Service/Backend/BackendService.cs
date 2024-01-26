using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend.Processor;
using Rich.Base.Runtime.Concrete.Promise;
using Rich.Base.Runtime.Signals;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Service.Backend
{
    public delegate void ExtensionCallback();

    public delegate void ProgressCallback(long progress);

    public class BackendService : IBackendService, IProcessorService
    {
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }

        [Inject] public BackendServiceSignals Signals { get; set; }


        private string _serverUrl = "http://localhost:8080";

        public string SessionId { get; set; }

        public Dictionary<string, ProgressCallback> ProgressMap { get; set; }
        
        public string ServerUrl
        {
            get { return _serverUrl; }
        }

        private Dictionary<string, IRequestProcessor> _processorMap;

        public Dictionary<string, ExtensionCallback> ExtensionMap { get; set; }

        public List<string> ClearList { get; set; }

        public Dictionary<string, Promise.Promise> PromiseMap { get; set; }

        public Dictionary<string, object> ResponseMap { get; set; }

        [UsedImplicitly]
        [PostConstruct]
        public void OnPostConstruct()
        {
            ExtensionMap = new Dictionary<string, ExtensionCallback>();
            ProgressMap = new Dictionary<string, ProgressCallback>();
            ClearList = new List<string>();
            ResponseMap = new Dictionary<string, object>();
            PromiseMap = new Dictionary<string, Promise.Promise>();
            _processorMap = new Dictionary<string, IRequestProcessor>();
        }

        public void AddProcessor(IRequestProcessor processor)
        {
            if (_processorMap.ContainsKey(processor.Command))
            {
                Debug.LogWarning("Data Service already has proccessor for " + processor.Command);
                return;
            }

            processor.Service = this;
            _processorMap.Add(processor.Command, processor);
        }


        public T GetData<T>(string command) where T : new()
        {
            if (ResponseMap.ContainsKey(command))
            {
                string response = ResponseMap[command].ToString();
                ResponseMap.Remove(command);
                return JsonConvert.DeserializeObject<T>(response);
            }

            return new T();
        }

        public string GetData(string command)
        {
            if (!ResponseMap.ContainsKey(command))
            {
                return string.Empty;
            }

            string response = ResponseMap[command].ToString();
            ResponseMap.Remove(command);
            return response;
        }

        /*
        public T GetEntity<T>(string command) where T : new()
        {
            var commands = command.Split('_');

            if (commands.Length > 1)
                command = commands[1];

            if (DataMap.ContainsKey(command))
            {
                var genericResponseVo = JsonConvert.DeserializeObject<GenericResponseVo>(DataMap[command].ToString());
                var deserializeObject = JsonConvert.DeserializeObject<T>(genericResponseVo.entity);
                return deserializeObject;
            }

            return new T();
        }*/

        public void Listen(string command, ExtensionCallback callback)
        {
            command = command.Split('_')[1];

            ListenInner(command, callback);
        }

        private void ListenInner(string command, ExtensionCallback callback)
        {
            if (ExtensionMap.ContainsKey(command))
            {
                Debug.Log("Already listening command: " + command);
                return;
            }

            ExtensionMap.Add(command, callback);
        }

        public void RemoveListen(string command)
        {
            command = command.Split('_')[1];

            if (ExtensionMap.ContainsKey(command))
            {
                ExtensionMap.Remove(command);
            }
        }

        public IPromise Request(string command, string data)
        {
            var splitList = command.Split('_');

            string newCommand, type = string.Empty;

            if (splitList.Length > 1)
            {
                type = splitList[0];
                newCommand = splitList[1];
            }
            else
            {
                newCommand = command;
            }

            newCommand = command;

            Debug.Log("Service.Request |> command: " + newCommand + " | type: " + type + "  | data: " + data);
            Promise.Promise promise = new Promise.Promise();
            if (PromiseMap.ContainsKey(newCommand))
            {
                PromiseMap[newCommand] = promise;
            }
            else
                PromiseMap.Add(newCommand, promise);

            if (string.IsNullOrEmpty(type))
            {
                if (_processorMap.ContainsKey(newCommand))
                    _processorMap[newCommand].Process(newCommand, data);
            }
            else
            {
                _processorMap[type].Process(newCommand, data);
            }
            return promise;
        }

        public IPromise Request(string command, object data)
        {
            return Request(command, JsonConvert.SerializeObject(data));
        }

        public IPromise Request(string command)
        {
            return Request(command, "{}");
        }

        // main sending part, missing 
        public void Send(string command, string data)
        {
            command = command.Split('_')[1];
            Debug.Log(command + " - " + data);
        }

        public void Send(string command, object data)
        {
            Send(command, JsonConvert.SerializeObject(data));
        }

        public void SetResponse(string command, object data)
        {
            ResponseMap[command] = JsonConvert.SerializeObject(data);

            if (ExtensionMap.ContainsKey(command))
                ExtensionMap[command]();

            if (PromiseMap.ContainsKey(command))
            {
                Promise.Promise promise = PromiseMap[command];
                PromiseMap.Remove(command);
                promise.Resolve();
            }
            Signals.ResponseHelper.Dispatch(command);
            //dispatcher.Dispatch(commandType, command);
        }

        public void AddProgressBarListener(string key, ProgressCallback callback)
        {
            ProgressMap[key] = callback;
        }

        public void RemoveProgressBarListener(string key)
        {
            if (ProgressMap.ContainsKey(key))
                ProgressMap.Remove(key);
        }

        public void UpdateProgressBar(string key, long progress)
        {
            ProgressMap[key](progress);
        }

        [UsedImplicitly]
        [Deconstruct]
        public void OnDeconstruct()
        {

        }
    }
}