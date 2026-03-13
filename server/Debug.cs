
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ValheimRestApi.Server
{
    public class RequestData
    {
        [JsonProperty("playerName")]
        public string playerName { get; set; }
    }

    public static class Debug
    {
        public static async Task<object> Test(object sender, EventArgs args)
        {
            await Task.Delay(2000);

            if (args is HttpEventArgs dataArgs)
            {
                var data = JsonParser.ParseRequest<RequestData>(dataArgs.Request);

                string playerName = data.playerName;
                return new { test = "yes", playerName = playerName };
            }

            return new { test = "no data" };
        }
    }
}
