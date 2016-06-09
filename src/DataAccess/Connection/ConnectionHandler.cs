using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Connection
{
    public class ConnectionHandler : IConnectionHandler
    {
        private IDbConnection _connection;
        private ITransactionWrapper _transactionWrapper;

        public void Dispose()
        {
            if (ConnectionIsAlive && !_transactionWrapper.TransactionInProgress)
            {
                _connection.Close();
            }
        }

        public IConnectionHandler GetHandler(IDbCommand command, ITransactionWrapper transactionWrapper)
        {
            _connection = command.Connection;
            _transactionWrapper = transactionWrapper;
            return this;
        }

        public void RollbackTransactionAndCloseConnection()
        {
            if (_transactionWrapper != null && _transactionWrapper.TransactionInProgress)
                _transactionWrapper.Rollback();

            if (ConnectionIsAlive)
            {
                _connection.Close();
            }
        }

        private bool ConnectionIsAlive
        {
            get { return _connection != null && _connection.State != ConnectionState.Closed; }
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }
    }
}