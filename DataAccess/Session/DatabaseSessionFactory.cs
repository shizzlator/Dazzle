using DataAccess.Command;
using DataAccess.Interfaces;

namespace DataAccess.Session
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
            var databaseCommandCreator = new DatabaseCommandBuilder(databaseConnectionManager);
            return new DatabaseSession(databaseCommandCreator, transactionManager);   
        }
    }
}
