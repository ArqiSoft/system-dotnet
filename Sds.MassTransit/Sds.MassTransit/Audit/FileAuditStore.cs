using MassTransit.Audit;
using Newtonsoft.Json;
using Serilog;
using System.Threading.Tasks;

namespace Sds.MassTransit.Audit
{
    public class FileAuditStore : IMessageAuditStore
    {
        public Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
            Log.Information($"[Audit] {metadata.ContextType} {typeof(T).FullName}: {JsonConvert.SerializeObject(message)} Metadata: {JsonConvert.SerializeObject(metadata, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })}");

            return Task.CompletedTask;
        }
    }
}
