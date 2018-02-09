using Newtonsoft.Json;
using System;

namespace CAApi.Models
{
    public class ApiError
    {
        public string Message { get; set; }
        public string Detail { get; set;  }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StackTrace { get; set; }
    }
}
