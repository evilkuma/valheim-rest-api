using BepInEx;
using Newtonsoft.Json;
using Shared;

namespace ValheimRestApi
{
    public class DebugRpcData
    {
        [JsonProperty("message")]
        public string message { get; set; }
    }

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
            var data = JsonParser.ParsePkg<DebugRpcData>(pkg);
            Log.LogInfo($"Получили тестовое сообщение от сервера: {data.message}");

            ZPackage package = new ZPackage();
            package.Write("{ status: \"ok\" }");
            RpcManager.SendMessage("ValheimRestApi/api/test", RpcManager.GetServerId(), package);
        }
    }
}