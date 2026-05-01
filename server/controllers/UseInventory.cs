
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class UseInventory : HttpController<InventoryData.ActionMainData>
    {
        public UseInventory() : base()
        {
            http = "/api/inventory";
        }

        protected override async Task<object> Action(InventoryData.ActionMainData data)
        {   
            var targetPeer = RpcManager.FindPlayerByName(data.playerName);
            if (targetPeer == null) return new { error = "no player peer" };

            var zData = await RpcManager.SendMessageAsync(InventoryData.rpc, targetPeer.m_uid,
                new RpcRequestData<InventoryData.RpcRequestMainData>
                {
                    action = "get-inventory",
                    data = new InventoryData.RpcRequestMainData {}
                }
            ).Task;
            return JsonParser.Parse<InventoryData.RpcResponseData>(zData);
        }
    }
}
