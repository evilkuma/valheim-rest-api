
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;

namespace ValheimRestApi.Server
{
    // TODO: возможно сделать общее хранилище для типов
    public class RequestData
    {
        [JsonProperty("playerName")]
        public string playerName { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class DebugRpcData
    {
        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class DebugRpcAnswerData
    {
        [JsonProperty("status")]
        public string message { get; set; }
    }

    public static class Debug
    {
        public static async Task<object> Test(object sender, EventArgs args)
        {
            if (args is HttpEventArgs dataArgs)
            {
                var data = JsonParser.ParseRequest<RequestData>(dataArgs.Request);

                string playerName = data.playerName;
                string message = data.message;

                var targetPeer = RpcManager.FindPlayerByName(playerName);
                if (targetPeer == null) return new { error = "no player peer" };

                ZPackage package = new ZPackage();
                package.Write($"{{ message: \"{message}\" }}");

                var zData = await RpcManager.SendMessageAsync("ValheimRestApi/api/test", targetPeer.m_uid, package).Task;
                return JsonParser.ParsePkg<DebugRpcAnswerData>(zData);
            }

            return new { error = "no data" };
        }
    }
}
