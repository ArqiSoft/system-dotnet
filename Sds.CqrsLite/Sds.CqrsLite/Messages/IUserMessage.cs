using CQRSlite.Messages;
using System;

namespace Sds.CqrsLite.Messages
{
    public interface IUserMessage : IMessage
    {
        Guid UserId { get; }
    }
}
