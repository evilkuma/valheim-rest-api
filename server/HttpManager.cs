

namespace ValheimRestApi.Server
{
    public static class HttpManager
    {
        public static void Initialize()
        {
            ServerValheimRestAPIPlugin.httpManager.Add("/api/test", ValheimRestApi.Server.Debug.Test);
        }
    }
}
