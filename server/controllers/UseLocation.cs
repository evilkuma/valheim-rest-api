
using System.Threading.Tasks;
using Shared;
using Shared.Models;
using UnityEngine;

namespace ValheimRestApi.Server
{
    public class UseLocation : HttpController
    {
        public UseLocation() : base()
        {
            http = "/api/location";
            RegisterAction<LocationData.ActionTeleportToBiomeData>("teleport-to-biome", ActionTeleportToBiome);
        }

        private async Task<object> ActionTeleportToBiome(LocationData.ActionTeleportToBiomeData data)
        {
            string playerName = data.playerName;
            string biomeName = data.biome;

            var targetPeer = RpcManager.FindPlayerByName(playerName);
            if (targetPeer == null) return new { error = "no player peer" };

            Vector3 playerPosition = targetPeer.m_refPos;
            Vector3 targetPosition = new Vector3();
            float distance = float.MaxValue;

            foreach (var entry in ZoneSystem.instance.m_locationInstances)
            {
                var location = entry.Value.m_location;
                var position = entry.Value.m_position;
                string biome = location.m_biome.ToString();

                if (!biome.Contains(biomeName)) continue;

                float numX = position.x - playerPosition.x;
                float numZ = position.z - playerPosition.z;

                float dist = numX * numX + numZ * numZ;

                if (dist > distance) continue;

                distance = dist;
                targetPosition = position;
            }

            if (distance == float.MaxValue) return new { error = "no finded location" };

            var zData = await RpcManager.SendMessageAsync(LocationData.rpc, targetPeer.m_uid,
                new RpcRequestData<LocationData.ActionTeleportToData>
                {
                    action = "teleport-to",
                    data = new LocationData.ActionTeleportToData {
                        x = targetPosition.x,
                        y = targetPosition.y,
                        z = targetPosition.z
                    }
                }
            ).Task;
            return JsonParser.Parse<LocationData.RpcResponseData>(zData);
        }
    }
}
