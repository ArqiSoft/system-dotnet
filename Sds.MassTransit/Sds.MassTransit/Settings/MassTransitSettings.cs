namespace Sds.MassTransit.Settings
{
    public class MassTransitSettings
    {
        public string ConnectionString { get; set; } = "rabbitmq://guest:guest@localhost:5672";
        public ushort PrefetchCount { get; set; } = 16;
        public int ConcurrencyLimit { get; set; } = 16;
        public int ImmediateRetry { get; set; } = 100;
        public int RetryCount { get; set; } = 10;
        public int RetryInterval { get; set; } = 100;  //  milliseconds
        public int RedeliveryCount { get; set; } = 10;
        public int RedeliveryInterval { get; set; } = 100;  //  milliseconds
    }
}
