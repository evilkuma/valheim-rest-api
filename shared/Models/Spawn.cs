
using Newtonsoft.Json;

namespace Shared.Models
{
    public static class SpawnData
    {
        public static readonly string rpc = "ValheimRestApi/api/spawn";
        public static readonly string http = "/api/spawn";

        public class HttpData
        {
            [JsonProperty("playerName")]
            public string playerName { get; set; }
            [JsonProperty("prefabName")]
            public string prefabName { get; set; }
            [JsonProperty("amount")]
            public int amount { get; set; }
            [JsonProperty("level")]
            public int level { get; set; }
            [JsonProperty("pickup")]
            public bool pickup { get; set; }
        }
        
        public class RpcRequestData
        {
            [JsonProperty("prefabName")]
            public string prefabName { get; set; }
            [JsonProperty("amount")]
            public int amount { get; set; }
            [JsonProperty("level")]
            public int level { get; set; }
            [JsonProperty("pickup")]
            public bool pickup { get; set; }
        }

        public class RpcResponseData
        {
            [JsonProperty("status")]
            public string status { get; set; }
        }
    }
}