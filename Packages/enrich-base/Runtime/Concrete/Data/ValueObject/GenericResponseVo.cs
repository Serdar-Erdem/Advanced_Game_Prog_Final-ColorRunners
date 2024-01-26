using System;
using Newtonsoft.Json;
using Rich.Base.Runtime.Enums;

namespace Rich.Base.Runtime.Concrete.Data.ValueObject
{
    [Serializable]
    public class GenericResponseVo
    {
        public bool success;

        public string errorMessage;

        public ResponseCode responseCode;

        public string entity;

        public GenericResponseVo()
        {
            responseCode = ResponseCode.New;
        }

        public GenericResponseVo(bool response)
        {
            this.success = response;

            if (response)
                this.responseCode = ResponseCode.Success;
        }

        public T GetData<T>() where T : new()
        {
            if (entity == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(entity);
        }

        public void SetData(string data)
        {
            entity = data;
        }

        public void SetData(object data)
        {
            entity = JsonConvert.SerializeObject(data);
        }
    }
}
