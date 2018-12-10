using CQRSlite.Messages;
using System;

namespace Sds.CqrsLite.Messages
{
    public interface ICorrelatedMessage : IMessage
    {
        Guid CorrelationId { get; }
    }
}
