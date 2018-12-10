using CQRSlite.Events;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sds.EventStore.InMemory.Tests
{
    public class TestEvent : IEvent
    {
        public TestEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;

        public int Version { get; set; }
    }

    public class GenericTests
    {
        private InMemoryEventStore _store = new InMemoryEventStore();
        private Guid _id = Guid.NewGuid();

        public GenericTests()
        {
            _store.Save(new List<IEvent>() {
                new TestEvent(_id, "Event1") { Version = 1 },
                new TestEvent(_id, "Event2") { Version = 2 },
                new TestEvent(_id, "Event3") { Version = 3 }
            });
        }

        [Fact]
        public async Task ReadForward_AllEvents_ShouldReturnAllEventInExpectedOrder()
        {
            var events = await _store.ReadEventsForwardAsync(_id);

            events.Should().NotBeEmpty();
            events.Count().Should().Be(3);
            events.First().Version.Should().Be(1);
            events.Last().Version.Should().Be(3);
        }

        [Fact]
        public async Task ReadForward_FirstEvent_ShouldReturnOnlyTheFirstEvent()
        {
            var events = await _store.ReadEventsForwardAsync(_id, 0, 1);

            events.Should().NotBeEmpty();
            events.Count().Should().Be(1);
            events.First().Version.Should().Be(1);
        }

        [Fact]
        public async Task ReadForward_LatestEvent_ShouldReturnOnlyTheLatestEvent()
        {
            var events = await _store.ReadEventsForwardAsync(_id, 2, 1);

            events.Should().NotBeEmpty();
            events.Count().Should().Be(1);
            events.First().Version.Should().Be(3);
        }

        [Fact]
        public async Task ReadBackward_AllEvents_ShouldReturnAllEventInExpectedOrder()
        {
            var events = await _store.ReadEventsBackwardAsync(_id);

            events.Should().NotBeEmpty();
            events.Count().Should().Be(3);
            events.First().Version.Should().Be(3);
            events.Last().Version.Should().Be(1);
        }

        [Fact]
        public async Task ReadBackward_FirstEvent_ShouldReturnOnlyTheLatestEvent()
        {
            var events = await _store.ReadEventsBackwardAsync(_id, -1, 1);

            events.Should().NotBeEmpty();
            events.Count().Should().Be(1);
            events.First().Version.Should().Be(3);
        }

        [Fact]
        public async Task ReadBackward_LastEvent_ShouldReturnOnlyTheFirstEvent()
        {
            var events = await _store.ReadEventsBackwardAsync(_id, 0, 1);

            events.Should().NotBeEmpty();
            events.Count().Should().Be(1);
            events.First().Version.Should().Be(1);
        }
    }
}
