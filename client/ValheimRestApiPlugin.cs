using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ValheimRestApi.Client
{
    [BepInPlugin("ru.evilkuma.valheimrestapi.client", "Valheim Rest API Client", "1.0.0")]
    public class ClientValheimRestAPIPlugin : BaseUnityPlugin
    {
        public static ClientValheimRestAPIPlugin instance;
        
        private void Awake()
        {
            instance = this;
            
            Logger.LogInfo("=== Valheim Inventory API Client загружен ===");
        }


        private void OnDestroy()
        {
        }
    }
}