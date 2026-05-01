
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shared.Models
{
    public static class InventoryData
    {
        public static readonly string rpc = "ValheimRestApi/api/inventory";

        public class ActionMainData
        {
            [JsonProperty("playerName")]
            public string playerName { get; set; }
        }

        public class RpcRequestMainData {}

        public class Item
        {
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("prefabName")]
            public string prefabName { get; set; }
            [JsonProperty("stack")]
            public int stack { get; set; }
            [JsonProperty("quality")]
            public int quality { get; set; }
            [JsonProperty("durability")]
            public float durability { get; set; }
            [JsonProperty("isEquipable")]
            public bool isEquipable { get; set; }
        }

        public class RpcResponseData
        {
            [JsonProperty("items")]
            public List<Item> items { get; set; }
            [JsonProperty("count")]
            public int count { get; set; }
        }
    }
}