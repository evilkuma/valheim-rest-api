
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class UseSpawn : HttpController<SpawnData.ActionMainData>
    {
        public UseSpawn() : base()
        {
            http = "/api/spawn";
        }

        protected override async Task<object> Action(SpawnData.ActionMainData data)
        {
            var targetPeer = RpcManager.FindPlayerByName(data.playerName);
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
    }
}
