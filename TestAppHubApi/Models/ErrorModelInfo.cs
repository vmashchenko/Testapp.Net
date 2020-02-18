using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestAppApi.Models
{
    public class ErrorModelInfo : ResultModelBase
    {
        public ErrorModelInfo() 
            : base(false, StatusCodes.Status500InternalServerError)
        {
            Errors = new List<ErrorItem>();
        }

        [JsonPropertyName("errors")]
        public IList<ErrorItem> Errors { get; set; }

        public string Message { get; set; }

        [System.Text.Json.Serialization.JsonIgnore()]
        public object ErrorObject { get; set; }
    }

    public sealed class ErrorItem
    {
        [JsonPropertyName("error_code")]
        public string Type { get; set; }

        [JsonPropertyName("error_text")]
        public string Text { get; set; }
    }
}
