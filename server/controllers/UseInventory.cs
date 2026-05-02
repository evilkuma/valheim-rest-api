
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimStreamerApi.Server
{
    public class UseInventory : HttpController<InventoryData.ActionMainData>
    {
        public UseInventory() : base()
        {
            http = "/api/inventory";
            RegisterAction<InventoryData.ActionDisarmamentData>("disarmament", Disarmament);
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

        private async Task<object> Disarmament(InventoryData.ActionDisarmamentData data)
        {
            var targetPeer = RpcManager.FindPlayerByName(data.playerName);
            if (targetPeer == null) return new { error = "no player peer" };

            var zData = await RpcManager.SendMessageAsync(InventoryData.rpc, targetPeer.m_uid,
                new RpcRequestData<object>
                {
                    action = "disarmament",
                    data = new {}
                }
            ).Task;
            return JsonParser.Parse<InventoryData.RpcStatusData>(zData);
        }
    }
}
