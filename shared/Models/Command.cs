
using Newtonsoft.Json;

namespace Shared.Models
{
    public static class CommandData
    {
        public static readonly string rpc = "ValheimRestApi/api/command";
        public static readonly string http = "/api/command";

        public class HttpData
        {
            [JsonProperty("playerName")]
            public string playerName { get; set; }
            [JsonProperty("command")]
            public string command { get; set; }
            [JsonProperty("data")]
            public string data { get; set; }
        }

        public class RpcRequestData
        {
            [JsonProperty("command")]
            public string command { get; set; }
            [JsonProperty("data")]
            public string data { get; set; }
        }
        
        public class RpcResponseData
        {
            [JsonProperty("status")]
            public string status { get; set; }
        }
    }
}
