using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Command
{
    public class DatabaseCommandProvider : IDatabaseCommandProvider
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;
        private readonly ITransactionManager _transactionManager;
        private IDbConnection _dbConnection;

        public DatabaseCommandProvider(IDatabaseConnectionProvider databaseConnectionProvider, ITransactionManager transactionManager)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
            _transactionManager = transactionManager;
        }

        public IDbCommand CreateCommandForCurrentConnection()
        {
            _dbConnection = _databaseConnectionProvider.GetOpenConnection();
            return GetCommand();
        }

        private IDbCommand GetCommand()
        {
            var command = _dbConnection.CreateCommand();
            if (_transactionManager.TransactionInProgress)
                command.Transaction = _transactionManager.TransientTransaction;
            return command;
        }
    }
}