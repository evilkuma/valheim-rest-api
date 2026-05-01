
using Shared;
using Shared.Models;

namespace ValheimStreamerApi.Client
{
    public class Debug : RpcController
    {
        public Debug()
        {
            rpc = DebugData.rpc;
            RegisterAction<DebugData.RpcRequestMainData>("terminal-message", TerminalMessage);
        }

        private object TerminalMessage(DebugData.RpcRequestMainData data)
        {
            Log.LogInfo($"Получили тестовое сообщение от сервера: {data.message}");

            return new { status = "ok" };
        }
    }
}