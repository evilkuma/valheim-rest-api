
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Models;
using ValheimRestApi.Client.Commands;

namespace ValheimRestApi.Client
{
    public static class UseCommand
    {
        private static readonly Dictionary<string, Func<string, CommandData.RpcResponseData>> _commands = new ();

        public static void Initialize()
        {
            InitializeCommands();
            RpcManager.AddListener(CommandData.rpc, CommandRPC);
        }

        private static void InitializeCommands()
        {
            AddCommand(UndressCommand.Name, UndressCommand.Command);
        }

        private static void CommandRPC(ZPackage pkg)
        {
            var data = JsonParser.Parse<CommandData.RpcRequestData>(pkg);

            if (_commands.TryGetValue(data.command, out var func))
            {
                var result = func(data.data);
                ZPackage package = new ZPackage();
                package.Write(JsonParser.Serialize(result));
                RpcManager.SendMessage(CommandData.rpc, RpcManager.GetServerId(), package);
            }
            else
            {
                ZPackage package = new ZPackage();
                package.Write("{ status: \"Undefined command\" }");
                RpcManager.SendMessage(CommandData.rpc, RpcManager.GetServerId(), package);
            }
        }

        public static void AddCommand(string name, Func<string, CommandData.RpcResponseData> func)
        {
            _commands[name] = func;
            Log.LogInfo($"REGISTER COMMAND { name }");
        }
    }
}