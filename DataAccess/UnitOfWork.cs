using DataAccess.Interfaces;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IObjectContainer _container;
        private readonly ITransactionManager _transactionManager;

        public UnitOfWork(IObjectContainer container, ITransactionManager transactionManager)
        {
            _container = container;
            _transactionManager = transactionManager;
        }

        public void Commit()
        {
            _transactionManager.Commit();
        }

        public T Repository<T>() where T : IRepository
        {
            if (!_transactionManager.TransactionInProgress)
            {
                _transactionManager.Begin();
            }
            return _container.GetInstanceOf<T>();
        }

        public void Rollback()
        {
            _transactionManager.Rollback();
        }

        public void RollbackAndCloseConnection()
        {
            _transactionManager.RollbackAndDisposeConnection();
        }

        public void Dispose()
        {
            _transactionManager.Rollback();
        }
    }
}