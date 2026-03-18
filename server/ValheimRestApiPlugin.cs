using BepInEx;
using Shared;

namespace ValheimRestApi.Server
{
    [BepInPlugin("ru.evilkuma.valheimrestapi.server", "Valheim Rest API Server", "1.0.0")]
    public class ValheimRestAPIPlugin : BaseUnityPlugin
    {
        public static ValheimRestAPIPlugin instance;
        
        private HttpServer httpServer;

        private void Awake()
        {
            instance = this;

            Log.Initialize(base.Logger);
            RpcManager.Initialize();
            
            httpServer = new HttpServer(8080);
            httpServer.Start();

            Log.LogInfo("=== Valheim Rest API Server загружен ===");
            Log.LogInfo("HTTP Server: http://localhost:8080");
        }

        private void OnDestroy()
        {
            httpServer?.Dispose();
        }
    }
}