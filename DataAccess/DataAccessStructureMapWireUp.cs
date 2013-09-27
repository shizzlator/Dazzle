using DataAccess.Command;
using DataAccess.Interfaces;
using DataAccess.Session;
using StructureMap;

namespace DataAccess
{
    public class DataAccessStructureMapWireUp
    {
        public void Initialise(IContainer container, string connectionString)
        {
            container.Configure(x => x.For<IDatabaseSession>().Use<DatabaseSession>());
            container.Configure(x => x.For<IDatabaseCommandBuilder>().Use<DatabaseCommandBuilder>());
            container.Configure(x => x.For<IDatabaseCommandProvider>().Use<DatabaseCommandProvider>());
            container.Configure(x => x.For<IDatabaseConnectionProvider>().Use<SqlConnectionProvider>().Ctor<string>("connectionString").Is(connectionString));
            container.Configure(x => x.For<ITransactionManager>().Use<TransactionManager>());
            //container.Configure(x => x.For<IDataQueryBuilder>().Use<DataQueryBuilder>());
            container.Configure(x => x.For<IUnitOfWork>().Use<UnitOfWork>());
            container.Configure(x => x.For<IRepositoryContainer>().Use<RepositoryContainer>());
        }
    }
}