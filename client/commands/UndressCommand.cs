
using Shared.Models;
using UnityEngine;

namespace ValheimRestApi.Client.Commands
{
    public static class UndressCommand
    {
        public static readonly string Name = "undress";

        public static CommandData.RpcResponseData Command(string _json)
        {
            Player player = Player.m_localPlayer;

            if (player == null)
            {
                return new CommandData.RpcResponseData
                {
                    status = "not a player"
                };
            }

            var inventory = player.GetInventory();
            var allItems = inventory.GetAllItems();

            foreach (var item in allItems)
            {
                if (item.m_equipped)
                {
                    player.UnequipItem(item, true);
                }
            }

            return new CommandData.RpcResponseData
            {
                status = "ok"
            };
        }
    }
}