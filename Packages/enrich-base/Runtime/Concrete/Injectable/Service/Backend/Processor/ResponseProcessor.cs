using System;
using Newtonsoft.Json;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend;
using Rich.Base.Runtime.Abstract.Injectable.Service.Backend.Processor;
using Rich.Base.Runtime.Concrete.Data.ValueObject;
using Rich.Base.Runtime.Enums;
using strange.extensions.command.impl;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.Injectable.Service.Backend.Processor
{
    public class ResponseProcessor : EventCommand, IResponseProcessor
    {
        private string _command = "amazon";

        [Inject] public IBackendService backendService { get; set; }

        protected GenericResponseVo genericResponse;

        public override void Execute()
        {
            try
            {
                Command = (string)evt.data;
                genericResponse = backendService.GetData<GenericResponseVo>(Command);

                if (!genericResponse.success)
                {
                    if (genericResponse.responseCode == ResponseCode.New)
                        Debug.Log(Command.Split('_')[1].ToUpper() + " request is overrided by a promise ( It is not a bug or problem, just reminder )");
                    else
                        Debug.Log(JsonConvert.SerializeObject(genericResponse));
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw e;
            }
        }

        public virtual string Command
        {
            get { return _command; }
            set { _command = value; }
        }
    }
}
