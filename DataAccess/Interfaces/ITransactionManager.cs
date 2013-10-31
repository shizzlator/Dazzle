using System.Data;

namespace DataAccess.Interfaces
{
    public interface ITransactionManager
    {
        void Begin();
        bool TransactionInProgress { get; }
        IDbTransaction Transaction { get; }
        void Commit();
        void Rollback();
    }
}