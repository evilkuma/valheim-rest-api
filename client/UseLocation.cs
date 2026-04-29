
using Shared;
using Shared.Models;
using UnityEngine;

namespace ValheimRestApi.Client
{
    public static class UseLocation
    {
        public static void Initialize()
        {
            RpcManager.AddListener(LocationData.rpc, LocationRPC);
        }

        private static void LocationRPC(ZPackage pkg)
        {
            var data = JsonParser.Parse<LocationData.RpcRequestData>(pkg);

            var action = data.action;

            string status = "undefined action";

            if (action == "teleportTo")
            {
                status = TeleportTo(data.data);
            }

            ZPackage package = new ZPackage();
            package.Write($"{{ status: \"{status}\" }}");
            RpcManager.SendMessage(LocationData.rpc, RpcManager.GetServerId(), package);
        }

        private static string TeleportTo(string data)
        {
            var targetPosition = JsonParser.Parse<LocationData.ActionTeleportToData>(data);
            Player player = Player.m_localPlayer;

            if (player == null) return "not a player";

            player.TeleportTo(
                new Vector3(targetPosition.x, targetPosition.y, targetPosition.z),
                player.transform.rotation,
                false
            );

            return "ok";
        }
    }
}
