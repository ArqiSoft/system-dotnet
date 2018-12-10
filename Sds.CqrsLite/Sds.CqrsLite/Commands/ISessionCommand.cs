using CQRSlite.Commands;
using Sds.CqrsLite.Messages;

namespace Sds.CqrsLite.Commands
{
    public interface ISessionCommand : ICommand, ISessionMessage
    {
    }
}
