using System;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IConnectionHandler : IDisposable
    {
        IConnectionHandler GetHandler(IDbCommand command, ITransactionManager transactionManager);
        void CleanUp();
        IDbConnection Connection { get; }
    }
}