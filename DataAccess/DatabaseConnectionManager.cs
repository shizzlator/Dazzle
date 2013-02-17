using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseConnectionManager : IDatabaseConnectionManager
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;
        private readonly ITransactionManager _transactionManager;

        public DatabaseConnectionManager(IDatabaseConnectionProvider databaseConnectionProvider, ITransactionManager transactionManager)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
            _transactionManager = transactionManager;
        }

        public IDbConnection Connection { get; private set; }

        public IDbCommand CreateCommandForCurrentConnection()
        {
            InitialiseOrGetConnection();
            return GetCommand();
        }

        private void InitialiseOrGetConnection()
        {
            if (Connection == null)
            {
                Connection = _databaseConnectionProvider.GetOpenConnection();
                OpenConnectionIfClosed();
            }
        }

        private void OpenConnectionIfClosed()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        private IDbCommand GetCommand()
        {
            var command = Connection.CreateCommand();
            if (_transactionManager.TransactionInProgress)
                command.Transaction = _transactionManager.TransientTransaction;
            return command;
        }
    }
}