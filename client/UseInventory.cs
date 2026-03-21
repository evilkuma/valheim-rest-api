
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Client
{
    public static class UseInventory
    {
        public static void Initialize()
        {
            RpcManager.AddListener(InventoryData.rpc, GetInventory);
        }

        private static void GetInventory(ZPackage _pkg)
        {
            // Получаем локального игрока
            Player player = Player.m_localPlayer;

            if (player == null)
            {
                Log.LogError("Игрок не найден.");
                return;
            }
            
            // Получаем инвентарь
            Inventory inventory = player.GetInventory();
            
            if (inventory == null)
            {
                Log.LogError("Инвентарь пуст или недоступен.");
                return;
            }

            // Получаем список предметов
            List<ItemDrop.ItemData> items = inventory.GetAllItems();
            
            // Преобразуем в простой формат для JSON (DTO)
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
            
            // Сериализуем в JSON
            string json = JsonParser.Serialize(inventoryData);
            
            // Упаковываем и отправляем ответ на сервер
            ZPackage package = new ZPackage();
            package.Write(json);
            RpcManager.SendMessage(InventoryData.rpc, RpcManager.GetServerId(), package);
        }
    }
}
