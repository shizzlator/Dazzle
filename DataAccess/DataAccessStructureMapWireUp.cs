using DataAccess.Interfaces;
using StructureMap;

namespace DataAccess
{
    public class DataAccessStructureMapWireUp
    {
        public void Initialise(IContainer container, string connectionString)
        {
            container.Configure(x => x.For<IDatabaseSession>().Use<DatabaseSession>());
            container.Configure(x => x.For<IDatabaseCommandFactory>().Use<DatabaseCommandFactory>());
            container.Configure(x => x.For<IDatabaseConnectionManager>().Use<DatabaseConnectionManager>());
            container.Configure(x => x.For<IDatabaseConnectionProvider>().Use<SqlConnectionProvider>().Ctor<string>("connectionString").Is(connectionString));
            container.Configure(x => x.For<ITransactionManager>().Use<TransactionManager>());
            container.Configure(x => x.For<IDataQueryBuilder>().Use<DataQueryBuilder>());
            container.Configure(x => x.For<IUnitOfWork>().Use<UnitOfWork>());
            container.Configure(x => x.For<IObjectContainer>().Use<ObjectContainer>());
        }

        public class ObjectContainer : IObjectContainer
        {
            public T GetInstanceOf<T>()
            {
                return ObjectFactory.Container.GetInstance<T>();
            }
        }
    }
}