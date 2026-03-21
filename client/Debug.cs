
using Shared;
using Shared.Models;

namespace ValheimRestApi.Client
{
    public static class Debug
    {
        public static void Initialize()
        {
            RpcManager.AddListener(DebugData.rpc, Test);
        }

        private static void Test(ZPackage pkg)
        {
            var data = JsonParser.ParsePkg<DebugData.RpcRequestData>(pkg);
            Log.LogInfo($"Получили тестовое сообщение от сервера: {data.message}");

            ZPackage package = new ZPackage();
            package.Write("{ status: \"ok\" }");
            RpcManager.SendMessage(DebugData.rpc, RpcManager.GetServerId(), package);
        }
    }
}