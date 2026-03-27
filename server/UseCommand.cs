
using System;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class UseCommand
    {
        public static async Task<object> CommandHttp(object sender, EventArgs args)
        {
            if (args is HttpEventArgs dataArgs)
            {
                var data = JsonParser.ParseRequest<CommandData.HttpData>(dataArgs.Request);
                
                string playerName = data.playerName;
                
                var targetPeer = RpcManager.FindPlayerByName(playerName);
                if (targetPeer == null) return new { error = "no player peer" };

                string json = JsonParser.Serialize(new CommandData.RpcRequestData
                {
                    command = data.command,
                    data = data.data
                });
                
                ZPackage package = new ZPackage();
                package.Write(json);

                var zData = await RpcManager.SendMessageAsync(CommandData.rpc, targetPeer.m_uid, package).Task;
                return JsonParser.ParsePkg<CommandData.RpcResponseData>(zData);
            }

            return new { error = "no data" };
        }
    }
}