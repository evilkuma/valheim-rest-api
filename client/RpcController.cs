
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Shared;
using Shared.Models;

namespace ValheimStreamerApi.Client
{
    public abstract class RpcController
    {
        public string rpc = "";
        private readonly Dictionary<string, Func<string, object>> _actions = new();

        public void Handle(ZPackage pkg)
        {
            string json = pkg.ReadString();
            var data = JsonParser.Parse<RpcRequestData<object>>(json);
            object result = new { status = "not action" };

            if (data.action != null && _actions.TryGetValue(data.action, out var handler))
            {
                result = handler(json);
            }

            RpcManager.SendMessage(rpc, RpcManager.GetServerId(), result);
        }

        protected void RegisterAction<TData>(string name, Func<TData, object> handler)
        {
            _actions[name.ToLower()] = json =>
            {
                var data = JsonParser.Parse<RpcRequestData<TData>>(json);
                return handler(data.data);
            };
        }

        public static void Init()
        {
            var controllerType = typeof(RpcController);
            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract && controllerType.IsAssignableFrom(t));
                
            foreach (var type in controllers)
            {
                var controller = (RpcController)Activator.CreateInstance(type);
                if (string.IsNullOrEmpty(controller.rpc))
                {
                    Log.LogError($"Controller {type.Name} has no rpc route, skipping");
                    continue;
                }
                RpcManager.AddListener(controller.rpc, controller.Handle);
                Log.LogInfo($"Registered controller: {type.Name} -> {controller.rpc}");
            }
        }
    }
}
