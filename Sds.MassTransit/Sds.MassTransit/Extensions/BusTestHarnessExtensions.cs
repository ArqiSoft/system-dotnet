using MassTransit.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Sds.MassTransit.Extensions
{
    public static class BusTestHarnessExtensions
    {
        public static async Task<IEnumerable<T>> Any<T>(this IReceivedMessageList received, TimeSpan? timeout = default(TimeSpan?)) where T : class
        {
            Expression<Func<IReceivedMessage<T>, bool>> filter = m => m.MessageType == typeof(T);

            return await received.Match<T>(filter);
        }

        public static async Task<IEnumerable<T>> Any<T>(this IReceivedMessageList received, Expression<Func<IReceivedMessage<T>, bool>> filter, TimeSpan? timeout = default(TimeSpan?)) where T : class
        {
            return await received.Match<T>(m => m.MessageType == typeof(T) && filter.Compile()(m));
        }

        public static async Task<IEnumerable<T>> Match<T>(this IReceivedMessageList published, Expression<Func<IReceivedMessage<T>, bool>> filter, TimeSpan? timeout = default(TimeSpan?)) where T : class
        {
            try
            {
                var giveUpAt = DateTime.Now + (timeout ?? TimeSpan.FromSeconds(30));

                while (DateTime.Now < giveUpAt)
                {
                    if (published.AsQueryable().Where(x => typeof(T).IsAssignableFrom(x.MessageType)).Cast<IReceivedMessage<T>>().Where(filter).Any())
                    {
                        return published.AsQueryable().Where(x => typeof(T).IsAssignableFrom(x.MessageType)).Cast<IReceivedMessage<T>>().Where(filter).Select(x => x.Context.Message).ToList();
                    }

                    await Task.Delay(10).ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException)
            {
                await Task.Delay(10).ConfigureAwait(false);

                return await published.Match<T>(filter, timeout);
            }

            return new T[] { };
        }

        public static async Task<IEnumerable<T>> Any<T>(this IPublishedMessageList published, TimeSpan? timeout = default(TimeSpan?)) where T : class
        {
            Expression<Func<IPublishedMessage<T>, bool>> filter = m => m.MessageType == typeof(T);

            return await published.Match<T>(filter);
        }

        public static async Task<IEnumerable<T>> Any<T>(this IPublishedMessageList published, Expression<Func<IPublishedMessage<T>, bool>> filter, TimeSpan? timeout = default(TimeSpan?)) where T : class
        {
            return await published.Match<T>(m => m.MessageType == typeof(T) && filter.Compile()(m));
        }

        public static async Task<IEnumerable<T>> Match<T>(this IPublishedMessageList published, Expression<Func<IPublishedMessage<T>, bool>> filter, TimeSpan? timeout = default(TimeSpan?)) where T : class
        {
            try
            {
                var giveUpAt = DateTime.Now + (timeout ?? TimeSpan.FromSeconds(30));

                while (DateTime.Now < giveUpAt)
                {
                    if (published.AsQueryable().Where(x => typeof(T).IsAssignableFrom(x.MessageType)).Cast<IPublishedMessage<T>>().Where(filter).Any())
                    {
                        return published.AsQueryable().Where(x => typeof(T).IsAssignableFrom(x.MessageType)).Cast<IPublishedMessage<T>>().Where(filter).Select(x => x.Context.Message).ToList();
                    }

                    await Task.Delay(10).ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException)
            {
                await Task.Delay(10).ConfigureAwait(false);

                return await published.Match<T>(filter, timeout);
            }

            return new T[] { };
        }

        public static IEnumerable<T> Select<T>(this IEnumerable<IPublishedMessage> published) where T : class
        {
            return published.Where(x => typeof(T).IsAssignableFrom(x.MessageType)).Cast<IPublishedMessage<T>>().Select(m => m.Context.Message);
        }

        public static async Task<bool> WaitWhileAllProcessed(this BusTestHarness harness, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = harness.TestTimeout;
            }

            var giveUpAt = DateTime.Now + timeout;
            while (DateTime.Now < giveUpAt)
            {
                try
                {
                    var allPublished = harness.Published.Count();
                    if (allPublished == harness.Consumed.Count())
                    {
                        for (var i = 0; i < 10 && allPublished == harness.Published.Count(); i++)
                        {
                            await Task.Delay(25).ConfigureAwait(false);
                        }

                        if (allPublished == harness.Published.Count())
                        {
                            return true;
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                }

                await Task.Delay(10).ConfigureAwait(false);
            }

            return false;
        }
    }
}
