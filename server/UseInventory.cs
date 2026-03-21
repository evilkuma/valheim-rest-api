
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class UseInventory
    {
        public static async Task<object> GetInventory(object sender, EventArgs args)
        {
            if (args is HttpEventArgs dataArgs)
            {
                var data = JsonParser.ParseRequest<InventoryData.HttpData>(dataArgs.Request);

                string playerName = data.playerName;

                var targetPeer = RpcManager.FindPlayerByName(playerName);
                if (targetPeer == null) return new { error = "no player peer" };

                ZPackage package = new ZPackage();

                var zData = await RpcManager.SendMessageAsync(InventoryData.rpc, targetPeer.m_uid, package).Task;
                return JsonParser.ParsePkg<InventoryData.RpcResponseData>(zData);
            }
            
            return new { error = "no data" };
        }
    }
}
