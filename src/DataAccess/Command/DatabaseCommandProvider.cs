using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Command
{
    internal class DatabaseCommandProvider : IDatabaseCommandProvider
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;
        private readonly ITransactionWrapper _transactionWrapper;

        public DatabaseCommandProvider(IDatabaseConnectionProvider databaseConnectionProvider, ITransactionWrapper transactionWrapper)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
            _transactionWrapper = transactionWrapper;
        }

        public IDbCommand CreateCommandForCurrentConnection()
        {
            var dbConnection = _databaseConnectionProvider.GetOpenConnection();
            var command = dbConnection.CreateCommand();
            if (_transactionWrapper.TransactionInProgress)
                command.Transaction = _transactionWrapper.Transaction;
            return command;
        }
    }
}