using CQRSlite.Events;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sds.CqrsLite.Moq
{
    public class MockEventPublisher : Mock<IEventPublisher>
    {
        public MockEventPublisher(Action<IEvent> action)
        {
            Setup(p => p.Publish<IEvent>(It.IsAny<IEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback<IEvent, CancellationToken>((e, t) => { action(e); });
        }
    }
}
