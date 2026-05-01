
using Shared;
using Shared.Models;
using UnityEngine;

namespace ValheimRestApi.Client
{
    public class UseLocation : RpcController
    {
        public UseLocation()
        {
            rpc = LocationData.rpc;
            RegisterAction<LocationData.ActionTeleportToData>("teleport-to", TeleportTo);
        }

        private object TeleportTo(LocationData.ActionTeleportToData data)
        {
            Player player = Player.m_localPlayer;

            if (player == null) return new { status = "not a player" };

            player.TeleportTo(
                new Vector3(data.x, data.y, data.z),
                player.transform.rotation,
                false
            );

            return new { status = "ok" };
        }
    }
}
