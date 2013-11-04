using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Connection
{
    internal class ConnectionHandler : IConnectionHandler
    {
        private IDbConnection _connection;
        private ITransactionManager _transactionManager;

        public void Dispose()
        {
            if (ConnectionIsAlive() && !_transactionManager.TransactionInProgress)
            {
                _connection.Close();
            }
        }

        public IConnectionHandler GetHandler(IDbCommand command, ITransactionManager transactionManager)
        {
            _connection = command.Connection;
            _transactionManager = transactionManager;
            return this;
        }

        public void CleanUp()
        {
            if (_transactionManager != null && _transactionManager.TransactionInProgress)
                _transactionManager.Rollback();

            if (ConnectionIsAlive())
            {
                _connection.Close();
            }
        }

        private bool ConnectionIsAlive()
        {
            return _connection != null && _connection.State == ConnectionState.Open;
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }
    }
}