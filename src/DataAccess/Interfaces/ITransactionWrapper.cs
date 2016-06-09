using System.Data;

namespace DataAccess.Interfaces
{
    public interface ITransactionWrapper
    {
        void Begin();
        bool TransactionInProgress { get; }
        IDbTransaction Transaction { get; }
        void Commit();
        void Rollback();
    }
}