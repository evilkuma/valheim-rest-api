
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ValheimRestApi.Models;
using Newtonsoft.Json;

namespace ValheimRestApi.Server
{
    public static class Debug
    {
        public static async Task<object> Test(object sender, EventArgs args)
        {
            await Task.Delay(2000);

            return new { test = "yes" };
        }
    }
}
