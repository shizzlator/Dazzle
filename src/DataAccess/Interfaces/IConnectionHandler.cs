using System;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IConnectionHandler : IDisposable
    {
        IConnectionHandler GetHandler(IDbCommand command, ITransactionWrapper transactionWrapper);
        void RollbackTransactionAndCloseConnection();
        IDbConnection Connection { get; }
    }
}