using CQRSlite.Commands;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sds.CqrsLite.Moq
{
    public class MockCommandSender : Mock<ICommandSender>
    {
        public MockCommandSender(Action<ICommand> action)
        {
            Setup(p => p.Send<ICommand>(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback<ICommand, CancellationToken>((c, t) => { action(c); });
        }
    }
}
