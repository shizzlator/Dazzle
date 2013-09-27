using DataAccess.Interfaces;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseSession _databaseSession;
        private readonly IRepositoryContainer _repositoryContainer;

        public UnitOfWork(IDatabaseSessionFactory databaseSessionFactory, IRepositoryContainer repositoryContainer)
        {
            _databaseSession = databaseSessionFactory.CreateSession();
            _repositoryContainer = repositoryContainer;
        }

        public void Commit()
        {
            _databaseSession.CommitTransaction();
        }

        public T Repository<T>() where T : IRepository
        {
            _databaseSession.BeginTransaction();
            return _repositoryContainer.GetInstanceOf<T>();
        }

        public T Repository<T>(string connectionString) where T : IRepository
        {
            _databaseSession.BeginTransaction();
            return _repositoryContainer.GetInstanceOf<T>();
        }

        public void Rollback()
        {
            _databaseSession.RollbackTransaction();
        }

        public void RollbackAndCloseConnection()
        {
            _databaseSession.RollbackTransaction();
            SqlConnectionProvider.Connection.Close();
        }
    }
}

