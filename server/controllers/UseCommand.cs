
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

            string json = JsonParser.Serialize(new CommandData.RpcRequestData
            {
                command = data.command,
                data = data.data
            });

            ZPackage package = new ZPackage();
            package.Write(json);

            var zData = await RpcManager.SendMessageAsync(CommandData.rpc, targetPeer.m_uid, package).Task;
            return JsonParser.Parse<CommandData.RpcResponseData>(zData);
        }
    }
}
