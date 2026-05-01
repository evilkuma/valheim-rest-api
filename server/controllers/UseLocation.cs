
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
            RegisterAction<LocationData.ActionTeleportToBiomeData>("teleportToBiome", ActionTeleportToBiome);
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

            string x = targetPosition.x.ToString().Replace(",", ".");
            string y = targetPosition.y.ToString().Replace(",", ".");
            string z = targetPosition.z.ToString().Replace(",", ".");

            string json = JsonParser.Serialize(new LocationData.RpcRequestData
            {
                action = "teleportTo",
                data = $"{{ \"x\": {x}, \"y\": {y}, \"z\": {z} }}"
            });

            ZPackage package = new ZPackage();
            package.Write(json);

            var zData = await RpcManager.SendMessageAsync(LocationData.rpc, targetPeer.m_uid, package).Task;
            return JsonParser.Parse<LocationData.RpcResponseData>(zData);
        }
    }
}
