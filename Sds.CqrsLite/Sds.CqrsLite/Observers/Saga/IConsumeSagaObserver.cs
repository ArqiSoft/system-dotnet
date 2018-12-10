using CQRSlite.Messages;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public interface IConsumeSagaObserver
    {
        Task PreConsume<T>(T message) where T : IMessage;

        Task PostConsume<T>(T message) where T : IMessage;

        Task ConsumeFault<T>(T message, Exception exception) where T : IMessage;
    }
}
