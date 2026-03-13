using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ValheimRestApi.Server;

namespace ValheimRestApi
{
    [BepInPlugin("ru.evilkuma.valheimrestapi.server", "Valheim Rest API Server", "1.0.0")]
    public class ServerValheimRestAPIPlugin : BaseUnityPlugin
    {
        public static ServerValheimRestAPIPlugin instance;
        public static readonly EventManager httpManager = new ();
        
        public ManualLogSource Log;
        private HttpServer httpServer;
        
        private void Awake()
        {
            instance = this;
            
            Log = base.Logger;
            
            // Запуск HTTP сервера (порт 8080)
            httpServer = new HttpServer(8080);
            httpServer.Start();

            HttpManager.Initialize();
            
            Logger.LogInfo("=== Valheim Rest API Server загружен ===");
            Logger.LogInfo("HTTP Server: http://localhost:8080");
        }

        private void OnDestroy()
        {
            httpServer?.Dispose();
        }
    }
}