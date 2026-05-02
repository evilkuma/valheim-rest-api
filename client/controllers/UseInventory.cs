
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Models;
using UnityEngine;

namespace ValheimStreamerApi.Client
{
    public class UseInventory : RpcController
    {
        public UseInventory()
        {
            rpc = InventoryData.rpc;
            RegisterAction<InventoryData.RpcRequestMainData>("get-inventory", GetInventory);
            RegisterAction<object>("disarmament", Disarmament);
        }

        private object GetInventory(InventoryData.RpcRequestMainData data)
        {
            Player player = Player.m_localPlayer;

            if (player == null)
            {
                return new { status = "not a player" };
            }
            
            Inventory inventory = player.GetInventory();
            
            if (inventory == null)
            {
                return new { status = "not a player inventory" };
            }

            List<ItemDrop.ItemData> items = inventory.GetAllItems();
            
            var itemsDto = items.Select(item => new InventoryData.Item
            {
                name = item.m_shared.m_name,
                prefabName = "",
                stack = item.m_stack,
                quality = item.m_quality,
                durability = item.m_durability,
                isEquipable = item.IsEquipable()
            }).ToList();
            var inventoryData = new InventoryData.RpcResponseData
            {
                items = itemsDto,
                count = items.Count
            };

            return inventoryData;
        }

        private object Disarmament(object _data)
        {
            Player player = Player.m_localPlayer;
            Inventory inventory = player.GetInventory();

            var weapons = inventory.GetAllItems()
                .Where(item => item.m_equipped && (
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon ||
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.TwoHandedWeapon ||
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow ||
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield ||
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet ||
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest ||
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs ||
                    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shoulder
                )).ToList();

            if (weapons.Count == 0)
                return new { status = "no weapons" };

            Vector3 center = player.transform.position;
            float radius = 3f;

            for (int i = 0; i < weapons.Count; i++)
            {
                var item = weapons[i];

                player.UnequipItem(item, triggerEquipEffects: false);

                float angle = 2 * Mathf.PI * i / weapons.Count;
                Vector3 position = new Vector3(
                    center.x + radius * Mathf.Cos(angle),
                    center.y + 1f,
                    center.z + radius * Mathf.Sin(angle)
                );

                if (item.m_dropPrefab != null)
                {
                    GameObject go = GameObject.Instantiate(item.m_dropPrefab, position, Quaternion.identity);
                    ItemDrop drop = go.GetComponent<ItemDrop>();
                    if (drop != null)
                    {
                        drop.m_itemData = item.Clone();

                        Rigidbody rb = go.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            Vector3 outward = (position - center).normalized;
                            rb.linearVelocity = outward * 4f + Vector3.up * 3f;
                        }
                    }
                }

                inventory.RemoveItem(item);
            }

            return new { status = "ok" };
        }
    }
}
