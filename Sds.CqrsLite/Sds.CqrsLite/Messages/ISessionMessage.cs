using CQRSlite.Messages;
using System;

namespace Sds.CqrsLite.Messages
{
    public interface ISessionMessage : IMessage
    {
        Guid SessionId { get; }
    }
}
