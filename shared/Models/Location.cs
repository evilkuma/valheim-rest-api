
/*
    Biome IDs:
    Meadows
    Swamp
    Mountain
    BlackForest
    Plains
    AshLands
    DeepNorth
    Ocean
    Mistlands
*/

using Newtonsoft.Json;

namespace Shared.Models
{
    public static class LocationData
    {
        public static readonly string rpc = "ValheimRestApi/api/location";

        public class RpcRequestData
        {
            [JsonProperty("action")]
            public string action { get; set; }
            [JsonProperty("data")]
            public string data { get; set; }
        }

        public class RpcResponseData
        {
            [JsonProperty("status")]
            public string status { get; set; }
        }

        // --------------
        // actions data
        // --------------
        
        public class ActionTeleportToBiomeData
        {
            [JsonProperty("playerName")]
            public string playerName { get; set; }
            [JsonProperty("biome")]
            public string biome { get; set; }
        }

        public class ActionTeleportToData
        {
            [JsonProperty("x")]
            public float x { get; set; }
            [JsonProperty("y")]
            public float y { get; set; }
            [JsonProperty("z")]
            public float z { get; set; }
        }
    }
}
