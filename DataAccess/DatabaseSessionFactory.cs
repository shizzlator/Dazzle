using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseSessionFactory : IDatabaseSessionFactory
    {
        private readonly string _connectionString;

        public DatabaseSessionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDatabaseSession CreateSession()
        {
            var sqlConnectionProvider = new SqlConnectionProvider(_connectionString);
            var transactionManager = new TransactionManager(sqlConnectionProvider);
            var databaseConnectionManager = new DatabaseCommandProvider(sqlConnectionProvider, transactionManager);
            var databaseCommandCreator = new DatabaseCommandCreator(databaseConnectionManager);
            return new DatabaseSession(databaseCommandCreator, transactionManager);   
        }
    }
}
