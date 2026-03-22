
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
                var data = JsonParser.ParseRequest<SpawnData.HttpData>(dataArgs.Request);
                
                string playerName = data.playerName;
                string prefabName = data.prefabName;

                var targetPeer = RpcManager.FindPlayerByName(playerName);
                if (targetPeer == null) return new { error = "no player peer" };

                string json = JsonParser.Serialize(new SpawnData.RpcRequestData
                {
                    prefabName = prefabName
                });

                ZPackage package = new ZPackage();
                package.Write(json);
                
                var zData = await RpcManager.SendMessageAsync(SpawnData.rpc, targetPeer.m_uid, package).Task;
                return JsonParser.ParsePkg<SpawnData.RpcResponseData>(zData);
            }
            
            return new { error = "no data" };
        }
    }
}