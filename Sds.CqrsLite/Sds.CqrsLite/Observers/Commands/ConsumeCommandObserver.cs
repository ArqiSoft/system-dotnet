using CQRSlite.Commands;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public class ConsumeCommandObserver : IConsumeCommandObserver
    {
        public Task ConsumeFault<T>(T command, Exception exception) where T : ICommand
        {
            Log.Error($"Cannot process command {command.GetType().FullName}. Error: {exception}");
            Log.Debug($"Command: { JsonConvert.SerializeObject(command)}");

            return Task.FromResult(true);
        }

        public Task PostConsume<T>(T command) where T : ICommand
        {
            Log.Information($"Command {command.GetType().FullName} processed successfully");
            Log.Debug($"Command: { JsonConvert.SerializeObject(command)}");

            return Task.FromResult(true);
        }

        public Task PreConsume<T>(T command) where T : ICommand
        {
            Log.Information($"Command {command.GetType().FullName} processing started");
            Log.Debug($"Command: { JsonConvert.SerializeObject(command)}");

            return Task.FromResult(true);
        }
    }
}
