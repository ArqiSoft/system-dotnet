using CQRSlite.Commands;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public interface IConsumeCommandObserver
    {
        Task PreConsume<T>(T command) where T : ICommand;

        Task PostConsume<T>(T command) where T : ICommand;

        Task ConsumeFault<T>(T command, Exception exception) where T : ICommand;
    }
}
