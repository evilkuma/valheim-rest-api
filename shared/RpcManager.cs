
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HarmonyLib;
using Shared.Models;

namespace Shared
{
    public static class RpcManager
    {
        public static readonly string[] RpcList = {
            DebugData.rpc,
            InventoryData.rpc,
            SpawnData.rpc,
            CommandData.rpc
        }; 

        private static readonly Dictionary<string, TaskCompletionSource<ZPackage>> _tasks = new ();
        private static readonly Dictionary<string, Action<ZPackage>> _events = new ();

        public static void Initialize()
        {
            Harmony.CreateAndPatchAll(typeof(RPCRegistration));
        }

        public static void AddListener(string name, Action<ZPackage> func)
        {
            _events[name] = func;
        }

        public static void Emit(string name, long sender, ZPackage pkg)
        {
            string taskId = $"{name}-{sender}";
            if (_tasks.TryGetValue(taskId, out var tcs))
            {
                tcs.SetResult(pkg);
            }

            if (_events.TryGetValue(name, out var func))
            {
                func(pkg);
            }
        }

        public static ZNetPeer FindPlayerByName(string playerName)
        {
            var peers = ZNet.instance.GetPeers();
            foreach (var peer in peers)
            {
                if (peer.m_playerName != null && 
                    peer.m_playerName.Equals(playerName))
                {
                    return peer;
                }
            }
            
            return null;
        }

        public static long GetServerId()
        {
            foreach (var peer in ZNet.instance.GetPeers())
            {
                if (peer.m_server)
                {
                    return peer.m_uid;
                }
            }

            return 0L;
        }

        public static TaskCompletionSource<ZPackage> SendMessageAsync(string eventName, long playerUID, ZPackage package)
        {
            var tcs = new TaskCompletionSource<ZPackage>();
            _tasks[$"{eventName}-{playerUID.ToString()}"] = tcs;

            ZRoutedRpc.instance.InvokeRoutedRPC(playerUID, eventName, package);

            return tcs;
        }

        public static void SendMessage(string eventName, long playerUID, ZPackage package)
        {
            ZRoutedRpc.instance.InvokeRoutedRPC(playerUID, eventName, package);
        }
    }

    public class RPCHandler
    {
        private string Name;

        public RPCHandler(string name)
        {
            Name = name;
        }

        public void Handle(long sender, ZPackage pkg)
        {
            RpcManager.Emit(Name, sender, pkg);
        }
    }

    [HarmonyPatch(typeof(Game), "Start")]
    public static class RPCRegistration
    {
        private static void Prefix()
        {
            Log.LogInfo("Регистрируем RPC");

            foreach (string name in RpcManager.RpcList)
            {
                RPCHandler handler = new (name);
                ZRoutedRpc.instance.Register(name, new Action<long, ZPackage>(handler.Handle));
            }
        }
    }
}
