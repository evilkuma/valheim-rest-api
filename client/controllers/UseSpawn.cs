
using Shared;
using Shared.Models;
using UnityEngine;

namespace ValheimStreamerApi.Client
{
    public class UseSpawn : RpcController
    {
        public UseSpawn()
        {
            rpc = SpawnData.rpc;
            RegisterAction<SpawnData.RpcRequestData>("main", Spawn);
            RegisterAction<object>("wooden-prison", WoodenPrison);
            RegisterAction<object>("stone-prison", StonePrison);
        }

        private object Spawn(SpawnData.RpcRequestData data)
        {
            var prefabName = data.prefabName;
            var amount = data.amount;
            var level = data.level;
            var pickup = data.pickup;

            Log.LogInfo($"Получили запрос на спавн {prefabName}");

            var status = SpawnPrefab(prefabName, amount, level, pickup);

            return new { status };
        }

        private object WoodenPrison(object _data)
        {
            string prefabName = "Beech1";
            int count = 10;
            float radius = 2.5f;

            GameObject prefab = ZNetScene.instance.GetPrefab(prefabName);
            if (!prefab) return new { status = $"Prefab not found: {prefabName}" };

            Player player = Player.m_localPlayer;
            Vector3 center = player.transform.position;

            for (int i = 0; i < count; i++)
            {
                float angle = 2 * Mathf.PI * i / count;
                Vector3 position = new Vector3(
                    center.x + radius * Mathf.Cos(angle),
                    center.y,
                    center.z + radius * Mathf.Sin(angle)
                );
                Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
                GameObject.Instantiate(prefab, position, rotation);
            }

            return new { status = "ok" };
        }

        private object StonePrison(object _data)
        {
            // stone_wall_2x1: ширина 2 юнита, высота 1 юнит
            const string wallPrefabName = "stone_wall_2x1";
            const int wallCount = 10;
            const int layers = 3;
            const float wallWidth = 2f;
            const float wallHeight = 1f;

            // Радиус подобран так, чтобы стены стояли вплотную без зазоров
            float radius = wallCount * wallWidth / (2 * Mathf.PI);

            GameObject prefab = ZNetScene.instance.GetPrefab(wallPrefabName);
            if (!prefab) return new { status = $"Prefab not found: {wallPrefabName}" };

            Player player = Player.m_localPlayer;
            Vector3 center = player.transform.position;

            for (int layer = 0; layer < layers; layer++)
            {
                float y = center.y + layer * wallHeight;

                for (int i = 0; i < wallCount; i++)
                {
                    float angle = 2 * Mathf.PI * i / wallCount;

                    Vector3 position = new Vector3(
                        center.x + radius * Mathf.Cos(angle),
                        y,
                        center.z + radius * Mathf.Sin(angle)
                    );

                    // Стена перпендикулярна радиусу — тангенциальное расположение
                    Quaternion rotation = Quaternion.Euler(0, angle * Mathf.Rad2Deg + 90f, 0);

                    GameObject go = GameObject.Instantiate(prefab, position, rotation);
                    go.GetComponent<Piece>()?.SetCreator(player.GetPlayerID());
                }
            }
            
            return new { status = "ok" };
        }

        private static string SpawnPrefab(string prefabName, int amount, int level, bool pickup)
        {
            GameObject prefab = ZNetScene.instance.GetPrefab(prefabName);
            if (!prefab)
            {
                return "Not find prefab";
            }

            if (prefab.GetComponent<Character>())
            {
                return SpawnCharacter(prefab, amount, level);
            }

            if (prefab.GetComponent<ItemDrop>())
            {
                return SpawnItem(prefab, amount, level, pickup);
            }
            
            SpawnItem(false, prefab);

            return "ok";
        }

        private static string SpawnCharacter(GameObject prefab, int amount, int level)
        {
            Player player = Player.m_localPlayer;
            Vector3 position = player.transform.position + player.transform.forward * 2f + Vector3.up;

            for (int i = 0; i < amount; i++)
            {
                GameObject spawnedChar = GameObject.Instantiate(prefab, position, Quaternion.identity);
                Character character = spawnedChar.GetComponent<Character>();

                character.SetLevel(level);
            }

            return "ok";
        }

        private static string SpawnItem(GameObject prefab, int amount, int level, bool pickup)
        {
            ItemDrop itemPrefab = prefab.GetComponent<ItemDrop>();

            if (itemPrefab.m_itemData.IsEquipable())
            {
                itemPrefab.m_itemData.m_quality = level;
                itemPrefab.m_itemData.m_durability = itemPrefab.m_itemData.GetMaxDurability();
                
                for (int i = 0; i < amount; i++)
                {
                    SpawnItem(pickup, prefab);
                }
            }
            else
            {
                int stacksCount = 1;
                int lastStack = 0;

                int maxStack = itemPrefab.m_itemData.m_shared.m_maxStackSize;
                
                if (maxStack < 1) maxStack = 1;

                itemPrefab.m_itemData.m_stack = maxStack;
                stacksCount = amount / maxStack;
                lastStack = amount % maxStack;

                for (int i = 0; i < stacksCount; i++)
                {
                    SpawnItem(pickup, prefab);
                }
                if (lastStack != 0)
                {
                    itemPrefab.m_itemData.m_stack = lastStack;
                    SpawnItem(pickup, prefab);
                }
                
            }

            itemPrefab.m_itemData.m_stack = 1;
            itemPrefab.m_itemData.m_quality = 1;

            return "ok";
        }

        private static GameObject SpawnItem(bool pickup, GameObject prefab)
        {
            Player player = Player.m_localPlayer;

            if (pickup)
            {
                player.PickupPrefab(prefab);
                return null;
            }
            else
                return GameObject.Instantiate(prefab, player.transform.position + player.transform.forward * 2f + Vector3.up, Quaternion.identity);
        }
    }
}