
using Newtonsoft.Json;

namespace Shared.Models
{
    public static class DebugData
    {
        public static readonly string rpc = "ValheimRestApi/api/debug";

        public class ActionMainData
        {
            [JsonProperty("playerName")]
            public string playerName { get; set; }
            [JsonProperty("message")]
            public string message { get; set; }
        }

        public class RpcRequestData
        {
            [JsonProperty("message")]
            public string message { get; set; }
        }

        public class RpcResponseData
        {
            [JsonProperty("status")]
            public string status { get; set; }
        }
    }
}