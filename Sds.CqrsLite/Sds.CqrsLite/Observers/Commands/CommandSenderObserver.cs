using CQRSlite.Commands;
using Newtonsoft.Json;
using Sds.CqrsLite.Commands;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public static class CommandExtentions
    {
        public static string ToStringFormat(this ICommand c)
        {
            var correlationId = c is ICorrelatedCommand ? $", CorrelationId: {(c as ICorrelatedCommand).CorrelationId}" : "";
            var userId = c is IUserCommand ? $", UserId: {(c as IUserCommand).UserId}" : "";

            return $"{c.GetType().FullName}({correlationId}{userId})";
        }
    }

    public class CommandSenderObserver : ICommandSenderObserver
    {
        public Task SendFault<T>(T command, Exception exception) where T : ICommand
        {
            Log.Error($"Cannot send command {command.ToStringFormat()}. Error: {exception}");
            Log.Debug($"Command: { JsonConvert.SerializeObject(command)}");

            return Task.FromResult(true);
        }

        public Task PostSend<T>(T command) where T : ICommand
        {
            Log.Information($"Command {command.ToStringFormat()} sent successfully");
            Log.Debug($"Command: { JsonConvert.SerializeObject(command)}");

            return Task.FromResult(true);
        }

        public Task PreSend<T>(T command) where T : ICommand
        {
            Log.Information($"Sending command {command.ToStringFormat()}");
            Log.Debug($"Command: { JsonConvert.SerializeObject(command)}");

            return Task.FromResult(true);
        }
    }
}
