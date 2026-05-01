
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Models;

namespace ValheimStreamerApi.Client
{
    public class UseCommand : RpcController
    {
        public UseCommand()
        {
            rpc = CommandData.rpc;
            RegisterAction<CommandData.RpcRequestMainData>("undress", Undress);
        }
        
        private object Undress(CommandData.RpcRequestMainData data)
        {
            Player player = Player.m_localPlayer;

            if (player == null)
            {
                return new { status = "not a player" };
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

            return new { status = "ok" };
        }
    }
}