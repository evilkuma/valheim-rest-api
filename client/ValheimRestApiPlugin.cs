using BepInEx;
using Shared;
using Shared.Models;

namespace ValheimStreamerApi.Client
{
    [BepInPlugin("ru.evilkuma.valheimstreamerapi.client", "Valheim Streamer API Client", "1.0.0")]
    public class ValheimStreamerAPIPlugin : BaseUnityPlugin
    {
        public static ValheimStreamerAPIPlugin instance;
        
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