using CQRSlite.Commands;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public interface ICommandSenderObserver
    {
        Task PreSend<T>(T command) where T : ICommand;

        Task PostSend<T>(T command) where T : ICommand;

        Task SendFault<T>(T command, Exception exception) where T : ICommand;
    }
}
