
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class UseSpawn
    {
        public static async Task<object> SpawnHttp(object sender, EventArgs args)
        {
            if (args is HttpEventArgs dataArgs)
            {
                var data = JsonParser.Parse<SpawnData.HttpData>(dataArgs.Request);
                
                string playerName = data.playerName;

                var targetPeer = RpcManager.FindPlayerByName(playerName);
                if (targetPeer == null) return new { error = "no player peer" };

                string json = JsonParser.Serialize(new SpawnData.RpcRequestData
                {
                    prefabName = data.prefabName,
                    amount = data.amount,
                    level = data.level,
                    pickup = data.pickup
                });

                ZPackage package = new ZPackage();
                package.Write(json);
                
                var zData = await RpcManager.SendMessageAsync(SpawnData.rpc, targetPeer.m_uid, package).Task;
                return JsonParser.Parse<SpawnData.RpcResponseData>(zData);
            }
            
            return new { error = "no data" };
        }
    }
}