using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Shared;

namespace ValheimRestApi.Server
{
    public abstract class HttpController
    {
        public string http = "";
        private readonly Dictionary<string, Func<HttpListenerRequest, Task<object>>> _actions = new();

        public async Task<object> Handle(object sender, EventArgs args)
        {
            if (args is not HttpEventArgs dataArgs)
                return new { error = "no data" };

            string action = dataArgs.Action == null ? "main" : dataArgs.Action.ToLower();

            if (_actions.TryGetValue(action, out var handler))
                return await handler(dataArgs.Request);

            return await HandleRequest(dataArgs);
        }

        protected void RegisterAction<TData>(string name, Func<TData, Task<object>> handler)
        {
            _actions[name.ToLower()] = async request =>
            {
                var data = JsonParser.Parse<TData>(request);
                return await handler(data);
            };
        }

        protected virtual Task<object> HandleRequest(HttpEventArgs args)
        {
            return Task.FromResult<object>(new { error = "undefined action" });
        }
    }

    public abstract class HttpController<TData> : HttpController
    {
        public HttpController()
        {
            RegisterAction<TData>("main", Action);
        }
        
        protected virtual Task<object> Action(TData data)
        {
            return Task.FromResult<object>(new { error = "undefined action" });
        }
    }
}
