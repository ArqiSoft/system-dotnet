using CQRSlite.Events;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public interface IEventPublisherObserver
    {
        Task PrePublish<T>(T @event) where T : IEvent;

        Task PostPublish<T>(T @event) where T : IEvent;

        Task PublishFault<T>(T @event, Exception exception) where T : IEvent;
    }
}
