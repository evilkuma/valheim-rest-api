using BepInEx;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Client
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

            RpcController.Init();

            Log.LogInfo("=== Valheim Inventory API Client загружен ===");
        }
    }
}