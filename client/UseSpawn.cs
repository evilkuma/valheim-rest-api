
using Shared;
using Shared.Models;
using UnityEngine;

namespace ValheimRestApi.Client
{
    public static class UseSpawn
    {
        public static void Initialize()
        {
            RpcManager.AddListener(SpawnData.rpc, SpawnRPC);
        }

        private static void SpawnRPC(ZPackage pkg)
        {
            var data = JsonParser.ParsePkg<SpawnData.RpcRequestData>(pkg);
            var prefabName = data.prefabName;

            Log.LogInfo($"Получили запрос на спавн {prefabName}");

            var status = SpawnPrefab(prefabName);

            ZPackage package = new ZPackage();
            package.Write($"{{ status: \"{status}\" }}");
            RpcManager.SendMessage(SpawnData.rpc, RpcManager.GetServerId(), package);
        }

        private static string SpawnPrefab(string prefabName)
        {
            int amount = 1;
            int level = 1;
            bool pickup = false;

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