using CQRSlite.Events;
using Sds.CqrsLite.Messages;

namespace Sds.CqrsLite.Events
{
    public interface ICorrelatedEvent : IEvent, ICorrelatedMessage, IUserMessage
    {
    }
}
