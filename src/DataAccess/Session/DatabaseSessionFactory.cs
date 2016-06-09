using DataAccess.Command;
using DataAccess.Connection;
using DataAccess.Interfaces;

namespace DataAccess.Session
{
    public class DatabaseSessionFactory : IDatabaseSessionFactory
    {
        private readonly IDatabaseConnectionProvider _connectionProvider;

        public DatabaseSessionFactory()
        {
        }

        public DatabaseSessionFactory(IDatabaseConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public IDatabaseSession CreateSession(string connectionString)
        {
            var sqlConnectionProvider = _connectionProvider ?? new SqlConnectionProvider(connectionString);
            var transactionWrapper = new TransactionWrapper(sqlConnectionProvider);
            var databaseConnectionManager = new DatabaseCommandProvider(sqlConnectionProvider, transactionWrapper);
            var databaseCommandCreator = new DatabaseCommandFactory(databaseConnectionManager);
            var databaseReaderFactory = new SqlDatabaseReaderFactory();
            var connectionHandler = new ConnectionHandler();
            return new DatabaseSession(databaseCommandCreator, transactionWrapper, databaseReaderFactory, connectionHandler);
        }
    }
}
