using Rich.Base.Runtime.Abstract.Injectable.Service.Backend;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend.Processor;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Service.Backend.Processor
{
    public class DefaultProcessor : IRequestProcessor
    {
        private string _command = "default";
        public IProcessorService Service { get; set; }

        //public IBackendService backendService;

        public void Process(string command, string data)
        {
            Command = "socket_" + command;
            Service.ClearList.Add(Command);

            Debug.Log("------>   command: " + command + " | data: " + data);
        }

        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }
    }
}