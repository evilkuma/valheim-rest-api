using BepInEx;
using BepInEx.Configuration;
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

            var port = Config.Bind(
                "Server",
                "Port",
                8080,
                "HTTP server port. Change requires server restart."
            );

            httpServer = new HttpServer(port.Value);
            httpServer.Start();

            Log.LogInfo("=== Valheim Rest API Server загружен ===");
            Log.LogInfo($"HTTP Server: http://localhost:{port.Value}");
        }

        private void OnDestroy()
        {
            httpServer?.Dispose();
        }
    }
}