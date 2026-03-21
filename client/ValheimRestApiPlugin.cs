using BepInEx;
using Shared;
using Shared.Models.Debug;

namespace ValheimRestApi
{
    [BepInPlugin("ru.evilkuma.valheimrestapi.client", "Valheim Rest API Client", "1.0.0")]
    public class ValheimRestAPIPlugin : BaseUnityPlugin
    {
        public static ValheimRestAPIPlugin instance;
        
        private void Awake()
        {
            instance = this;

            Log.Initialize(base.Logger);
            RpcManager.Initialize();

            RpcManager.AddListener("ValheimRestApi/api/test", OnTestMessage);

            Log.LogInfo("=== Valheim Inventory API Client загружен ===");
        }

        private void OnTestMessage(ZPackage pkg)
        {
            var data = JsonParser.ParsePkg<DebugRpcRequestData>(pkg);
            Log.LogInfo($"Получили тестовое сообщение от сервера: {data.message}");

            ZPackage package = new ZPackage();
            package.Write("{ status: \"ok\" }");
            RpcManager.SendMessage("ValheimRestApi/api/test", RpcManager.GetServerId(), package);
        }
    }
}