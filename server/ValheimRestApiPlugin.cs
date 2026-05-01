using BepInEx;
using BepInEx.Configuration;
using Shared;

namespace ValheimStreamerApi.Server
{
    [BepInPlugin("ru.evilkuma.valheimstreamerapi.server", "Valheim Streamer API Server", "1.0.0")]
    public class ValheimStreamerAPIPlugin : BaseUnityPlugin
    {
        public static ValheimStreamerAPIPlugin instance;

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

            Log.LogInfo("=== Valheim Streamer API Server загружен ===");
            Log.LogInfo($"HTTP Server: http://localhost:{port.Value}");
        }

        private void OnDestroy()
        {
            httpServer?.Dispose();
        }
    }
}