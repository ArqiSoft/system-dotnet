using CQRSlite.Events;
using Sds.CqrsLite.Messages;
using System;

namespace Sds.CqrsLite.Events
{
    public interface IUserEvent : IEvent, IUserMessage
    {
    }
}
