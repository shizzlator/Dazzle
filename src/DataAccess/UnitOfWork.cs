using DataAccess.Connection;
using DataAccess.Interfaces;
using DataAccess.Session;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseSession _databaseSession;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IDatabaseSessionFactory _databaseSessionFactory;

        public UnitOfWork(string connectionString) : this(new DatabaseSessionFactory(), new RepositoryFactory(), connectionString)
        {
        }

        internal UnitOfWork(IDatabaseSessionFactory databaseSessionFactory, IRepositoryFactory repositoryFactory, string connectionString)
        {
            _databaseSessionFactory = databaseSessionFactory;
            _databaseSession = _databaseSessionFactory.CreateSession(connectionString);
            _repositoryFactory = repositoryFactory;
        }

        public void Commit()
        {
            _databaseSession.CommitTransaction();
        }

        public void CommitAndCloseConnection()
        {
            _databaseSession.CommitTransaction();
            _databaseSession.Dispose();
        }

        public T Repository<T>() where T : IRepository
        {
            _databaseSession.BeginTransaction();
            return _repositoryFactory.GetInstanceOf<T>(_databaseSession);
        }

        public void Rollback()
        {
            _databaseSession.RollbackTransaction();
        }

        public void RollbackAndCloseConnection()
        {
            _databaseSession.RollbackTransaction();
			_databaseSession.Dispose();
        }

        public T Repository<T>(string connectionString) where T : IRepository
        {
            _databaseSession.BeginTransaction();
            return _repositoryFactory.GetInstanceOf<T>(_databaseSessionFactory.CreateSession(connectionString));
        }
    }
}

