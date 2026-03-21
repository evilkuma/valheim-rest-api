
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models.Debug;

namespace ValheimRestApi.Server
{
    public static class Debug
    {
        public static async Task<object> Test(object sender, EventArgs args)
        {
            if (args is HttpEventArgs dataArgs)
            {
                var data = JsonParser.ParseRequest<DebugHttpData>(dataArgs.Request);

                string playerName = data.playerName;
                string message = data.message;

                var targetPeer = RpcManager.FindPlayerByName(playerName);
                if (targetPeer == null) return new { error = "no player peer" };

                ZPackage package = new ZPackage();
                package.Write($"{{ message: \"{message}\" }}");

                var zData = await RpcManager.SendMessageAsync("ValheimRestApi/api/test", targetPeer.m_uid, package).Task;
                return JsonParser.ParsePkg<DebugRpcResponseData>(zData);
            }

            return new { error = "no data" };
        }
    }
}
