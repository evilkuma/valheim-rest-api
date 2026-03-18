
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Shared
{
    public static class JsonParser
    {
        public static TResult ParseRequest<TResult>(HttpListenerRequest request)
        {
            string requestBody;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                requestBody = reader.ReadToEnd();
            }

            var data = JsonConvert.DeserializeObject<TResult>(requestBody);

            return data;
        }

        public static TResult ParsePkg<TResult>(ZPackage pkg)
        {
            string jsonData = pkg.ReadString();
            var data = JsonConvert.DeserializeObject<TResult>(jsonData);

            return data;
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
    }
}