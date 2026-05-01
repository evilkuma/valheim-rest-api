
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class Debug : HttpController<DebugData.ActionMainData>
    {
        public Debug() : base()
        {
            http = "/api/debug";
        }

        protected override async Task<object> Action(DebugData.ActionMainData data)
        {
            var targetPeer = RpcManager.FindPlayerByName(data.playerName);
            if (targetPeer == null) return new { error = "no player peer" };

            ZPackage package = new ZPackage();
            package.Write($"{{ message: \"{data.message}\" }}");

            var zData = await RpcManager.SendMessageAsync(DebugData.rpc, targetPeer.m_uid, package).Task;
            return JsonParser.Parse<DebugData.RpcResponseData>(zData);
        }
    }
}
