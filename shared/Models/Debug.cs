
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

        public class RpcRequestMainData
        {
            [JsonProperty("message")]
            public string message { get; set; }
        }

        public class RpcResponseMainData
        {
            [JsonProperty("status")]
            public string status { get; set; }
        }
    }
}