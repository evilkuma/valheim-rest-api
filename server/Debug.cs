
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public static class Debug
    {
        public static async Task<object> Test(object sender, EventArgs args)
        {
            if (args is HttpEventArgs dataArgs)
            {
                var data = JsonParser.Parse<DebugData.HttpData>(dataArgs.Request);

                string playerName = data.playerName;
                string message = data.message;

                var targetPeer = RpcManager.FindPlayerByName(playerName);
                if (targetPeer == null) return new { error = "no player peer" };

                ZPackage package = new ZPackage();
                package.Write($"{{ message: \"{message}\" }}");

                var zData = await RpcManager.SendMessageAsync(DebugData.rpc, targetPeer.m_uid, package).Task;
                return JsonParser.Parse<DebugData.RpcResponseData>(zData);
            }

            return new { error = "no data" };
        }
    }
}
