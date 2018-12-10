using Automatonymous;
using MassTransit.Saga;

namespace Sds.MassTransit.Saga
{
    public interface ISagaRepositoryFactory
    {
        ISagaRepository<TInstance> Create<TInstance>() where TInstance : class, SagaStateMachineInstance;
    }
}
