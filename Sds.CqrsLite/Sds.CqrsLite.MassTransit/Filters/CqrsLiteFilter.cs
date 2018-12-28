﻿using CQRSlite.Events;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sds.CqrsLite.MassTransit.Filters
{
    public class CqrsLiteFilter<TConsumer> :
        IFilter<ConsumerConsumeContext<TConsumer>>
        where TConsumer : class

    {
        public void Probe(ProbeContext context)
        {
            var scope = context.CreateFilterScope("cqrslite");
        }

        public async Task Send(ConsumerConsumeContext<TConsumer> context, IPipe<ConsumerConsumeContext<TConsumer>> next)
        {
            try
            {
                var serviceScope = context.GetPayload<IServiceScope>();

                var publisher = serviceScope.ServiceProvider.GetRequiredService<IEventPublisher>() as MassTransitEventPublisher;

                if (publisher != null)
                {
                    publisher.SetContext(context);
                }

                await next.Send(context).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class CqrsLiteSpecification<TConsumer> : IPipeSpecification<ConsumerConsumeContext<TConsumer>>
        where TConsumer : class
    {
        public void Apply(IPipeBuilder<ConsumerConsumeContext<TConsumer>> builder)
        {
            builder.AddFilter(new CqrsLiteFilter<TConsumer>());
        }

        public IEnumerable<ValidationResult> Validate()
        {
            yield break;
        }
    }

    public static class CqrsLiteMiddlewareConfiguratorExtensions
    {
        public static void UseCqrsLite<TConsumer>(this IPipeConfigurator<ConsumerConsumeContext<TConsumer>> configurator) where TConsumer : class
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            var specification = new CqrsLiteSpecification<TConsumer>();

            configurator.AddPipeSpecification(specification);
        }
    }
}
