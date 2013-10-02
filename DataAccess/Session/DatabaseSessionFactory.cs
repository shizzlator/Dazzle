using DataAccess.Command;
using DataAccess.Connection;
using DataAccess.Interfaces;

namespace DataAccess.Session
{
    public class DatabaseSessionFactory : IDatabaseSessionFactory
    {
        private readonly string _connectionString;

        internal DatabaseSessionFactory()
        {
        }

        public DatabaseSessionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDatabaseSession CreateSession()
        {
            return CreateSession(_connectionString);
        }

        public IDatabaseSession CreateSession(string connectionString)
        {
            var sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            var transactionManager = new TransactionManager(sqlConnectionProvider);
            var databaseConnectionManager = new DatabaseCommandInstaceProvider(sqlConnectionProvider, transactionManager);
            var databaseCommandCreator = new DatabaseCommandFactory(databaseConnectionManager);
            var databaseReaderFactory = new SqlDatabaseReaderFactory();
            var connectionHandler = new ConnectionHandler();
            return new DatabaseSession(databaseCommandCreator, transactionManager, databaseReaderFactory, connectionHandler);
        }
    }
}
