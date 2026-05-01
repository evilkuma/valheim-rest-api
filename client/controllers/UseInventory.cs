
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Models;

namespace ValheimStreamerApi.Client
{
    public class UseInventory : RpcController
    {
        public UseInventory()
        {
            rpc = InventoryData.rpc;
            RegisterAction<InventoryData.RpcRequestMainData>("get-inventory", GetInventory);
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
    }
}
