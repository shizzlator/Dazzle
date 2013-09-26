using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseSessionFactory : IDatabaseSessionFactory
    {
        public IDatabaseSession CreateSession(string connectionString)
        {
            SqlConnectionProvider sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            TransactionManager transactionManager = new TransactionManager(sqlConnectionProvider);
            DatabaseConnectionManager dbConnectionManager = new DatabaseConnectionManager(sqlConnectionProvider, transactionManager);
            DatabaseCommandFactory dbCommandFactory = new DatabaseCommandFactory(dbConnectionManager);
            return new DatabaseSession(dbCommandFactory, transactionManager);   
        }
    }
}
