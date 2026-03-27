using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shared;
using Shared.Models;

namespace ValheimRestApi.Server
{
    public class HttpEventArgs : EventArgs
    {
        public HttpListenerRequest Request { get; }
        public HttpListenerResponse Response { get; }

        public HttpEventArgs(HttpListenerRequest request, HttpListenerResponse response)
        {
            Request = request;
            Response = response;
        }
    }

    public class HttpServer : IDisposable
    {
        private static readonly EventManager Events = new ();

        private HttpListener listener;
        private Thread serverThread;
        private bool isRunning;

        public HttpServer(int port)
        {
            Events.Add(DebugData.http, ValheimRestApi.Server.Debug.Test);
            Events.Add(InventoryData.http, ValheimRestApi.Server.UseInventory.GetInventory);
            Events.Add(SpawnData.http, ValheimRestApi.Server.UseSpawn.SpawnHttp);
            Events.Add(CommandData.http, ValheimRestApi.Server.UseCommand.CommandHttp);

            listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{port}/");
            listener.Prefixes.Add($"http://localhost:{port}/");
        }

        public void Start()
        {
            if (!HttpListener.IsSupported)
            {
                Log.LogError("HttpListener не поддерживается");
                return;
            }

            try
            {
                listener.Start();
                isRunning = true;
                serverThread = new Thread(async () => await HandleRequests());
                serverThread.Start();
                
                Log.LogInfo("HTTP сервер запущен");
            }
            catch (Exception ex)
            {
                Log.LogError($"Ошибка запуска: {ex.Message}");
            }
        }

        private async Task HandleRequests()
        {
            while (isRunning)
            {
                try
                {
                    var context = await listener.GetContextAsync();
                    _ = Task.Run(() => ProcessRequest(context));
                }
                catch (Exception ex)
                {
                    Log.LogError($"Ошибка обработки: {ex.Message}");
                }
            }
        }

        private async Task ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            try
            {
                // CORS заголовки
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                response.ContentType = "application/json";

                if (request.HttpMethod == "OPTIONS")
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Close();
                    return;
                }

                string eventName = request.Url.AbsolutePath.ToLower();
                if (Events.HasEvent(eventName))
                {
                    object result = await Events.Dispatch<object>(request.Url.AbsolutePath.ToLower(), this, new HttpEventArgs(request, response));
                    await WriteJsonResponse(response, result);
                }
                else
                {
                    response.StatusCode = 404;
                    await WriteJsonResponse(response, new { error = "Endpoint not found" });
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Ошибка ProcessRequest: {ex.Message}");
                response.StatusCode = 500;
                await WriteJsonResponse(response, new { error = ex.Message });
            }
            finally
            {
                response.Close();
            }
        }

        private async Task WriteJsonResponse(HttpListenerResponse response, object data)
        {
            string json = JsonParser.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        public void Dispose()
        {
            isRunning = false;
            serverThread?.Join(1000);
        }
    }
}