
using Newtonsoft.Json;

namespace Shared.Models.Debug
{
    public class DebugHttpData
    {
        [JsonProperty("playerName")]
        public string playerName { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class DebugRpcRequestData
    {
        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class DebugRpcResponseData
    {
        [JsonProperty("status")]
        public string message { get; set; }
    }
}