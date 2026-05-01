
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class UseCommand : HttpController<CommandData.ActionMainData>
    {
        public UseCommand() : base()
        {
            http = "/api/command";
        }

        protected override async Task<object> Action(CommandData.ActionMainData data)
        {
            var targetPeer = RpcManager.FindPlayerByName(data.playerName);
            if (targetPeer == null) return new { error = "no player peer" };

            var zData = await RpcManager.SendMessageAsync(CommandData.rpc, targetPeer.m_uid,
                new RpcRequestData<CommandData.RpcRequestMainData>
                {
                    action = data.command,
                    data = new CommandData.RpcRequestMainData {}
                }
            ).Task;
            return JsonParser.Parse<CommandData.RpcResponseData>(zData);
        }
    }
}
