using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Serialization;

namespace TestAppApi.Models
{
    public abstract class ResultModelBase
    {
        protected ResultModelBase(bool isSuccess, int httpStatus)
        {
            IsSuccess = isSuccess;
            StatusCode = httpStatus;
        }

        [System.Text.Json.Serialization.JsonIgnore()]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("status")]
        public int StatusCode { get; set; }
        
        public string Success => IsSuccess.ToString();

        public override string ToString()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serializerOpts = new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            };

            var json = JsonConvert.SerializeObject(this, serializerOpts);
            return json;
        }
    }

    public class BaseVm
    {        
        public int RequestId { get;set; }
    }

    public abstract class ResultModelBase<T> : ResultModelBase
    {
        protected ResultModelBase(bool status, int httpStatus) 
            : base(status, httpStatus)
        {
        }

        public T Data { get; set; }
    }
}
