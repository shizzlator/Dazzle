using DataAccess.Command;
using DataAccess.Interfaces;

namespace DataAccess.Session
{
    public class DatabaseSessionFactory : IDatabaseSessionFactory
    {
        private readonly string _connectionString;

        public DatabaseSessionFactory()
        {
        }

        internal DatabaseSessionFactory(string connectionString)
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
            var databaseConnectionManager = new DatabaseCommandProvider(sqlConnectionProvider, transactionManager);
            var databaseCommandCreator = new DatabaseCommandBuilder(databaseConnectionManager);
            var databaseReaderFactory = new SqlDatabaseReaderFactory();
            return new DatabaseSession(databaseCommandCreator, transactionManager, databaseReaderFactory);
        }
    }
}
