
// using System;
// using System.Collections.Generic;
// using ValheimRestApi.Models;

// namespace ValheimRestApi.Server
// {
//     public class RpcManager
//     {
//         public event EventHandler<InventoryEventArgs> InventoryEvent;
//         // public event EventHandler<InventoryEventArgs> InventoryDataEvent;
//         // public event EventHandler<InventoryEventArgs> PlayerStatusEvent;

//         public RpcManager()
//         {
//             ZRoutedRpc.instance.Register<ZPackage>("ValheimRestApi.GetInventory", OnInventoryEvent);
//             // ZRoutedRpc.instance.Register<ZPackage>("ValheimRestApi.InventoryData", OnInventoryEvent);
//             // ZRoutedRpc.instance.Register<ZPackage>("ValheimRestApi.PlayerStatus", OnInventoryEvent);
//         }

//         protected virtual void OnInventoryEvent(long senderId, ZPackage pkg)
//         {
//             try
//             {
//                 string requestId = pkg.ReadString();
//                 string playerName = pkg.ReadString();
//                 int status = pkg.ReadInt();
                
//                 int dataCount = pkg.ReadInt();
//                 Dictionary<string, object> Data = new Dictionary<string, object>();

//                 for (int i = 0; i < dataCount; i++)
//                 {
//                     string key = pkg.ReadString();
//                     string value = pkg.ReadString();
//                     Data[key] = value;
//                 }

//                 //requestQueue.HandleResponse(response);
//                 InventoryEvent?.Invoke(this, new InventoryEventArgs(requestId, playerName, (ResponseStatus)status, DateTime.UtcNow, Data));
//             }
//             catch (Exception ex)
//             {
//                 ServerValheimRestAPIPlugin.instance.Log.LogError($"Ошибка OnInventoryEvent: {ex.Message}");
//             }
//         }
        
//         // protected virtual void OnInventoryDataEvent()
//         // {
//         //     InventoryDataEvent?.Invoke(this, new InventoryEventArgs("data"));
//         // }
        
//         // protected virtual void OnPlayerStatusEvent()
//         // {
//         //     PlayerStatusEvent?.Invoke(this, new InventoryEventArgs("data"));
//         // }
//     }
// }
