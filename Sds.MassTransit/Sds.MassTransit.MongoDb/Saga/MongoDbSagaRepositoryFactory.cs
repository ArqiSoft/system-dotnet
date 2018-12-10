using Automatonymous;
using MassTransit.MongoDbIntegration.Saga;
using MassTransit.Saga;
using Sds.MassTransit.Saga;
using System;

namespace Sds.MassTransit.MongoDb.Saga
{
    public class MongoDbSagaRepositoryFactory : ISagaRepositoryFactory
    {
        private string _connectionString;
        private string _collectionName;

        public MongoDbSagaRepositoryFactory(string connectionString, string collectionName = "sagas")
        {
            _connectionString = connectionString;
            _collectionName = collectionName;
        }

        public ISagaRepository<TInstance> Create<TInstance>() where TInstance : class, SagaStateMachineInstance
        {
            var repository = typeof(MongoDbSagaRepository<>);
            var makeme = repository.MakeGenericType(new Type[] { typeof(TInstance) });
            return Activator.CreateInstance(makeme, new object[] { _connectionString, _collectionName, typeof(TInstance).Name }) as ISagaRepository<TInstance>;
        }
    }
}
