
using Newtonsoft.Json;

namespace Shared.Models
{
    public class RpcRequestData<TData>
    {
        [JsonProperty("action")]
        public string action { get; set; }
        [JsonProperty("data")]
        public TData data { get; set; }
    }
}
