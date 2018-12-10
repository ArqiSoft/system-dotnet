using CQRSlite.Events;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public interface IConsumeEventObserver
    {
        Task PreConsume<T>(T @event) where T : IEvent;

        Task PostConsume<T>(T @event) where T : IEvent;

        Task ConsumeFault<T>(T @event, Exception exception) where T : IEvent;
    }
}
