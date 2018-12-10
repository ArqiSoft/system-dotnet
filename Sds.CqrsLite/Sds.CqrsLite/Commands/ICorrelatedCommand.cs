using CQRSlite.Commands;
using Sds.CqrsLite.Messages;
using System;

namespace Sds.CqrsLite.Commands
{
    public interface ICorrelatedCommand : ICommand, ICorrelatedMessage, IUserMessage
    {
        Guid Id { get; }
    }
}
