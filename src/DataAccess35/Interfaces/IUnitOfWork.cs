namespace DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        T Repository<T>() where T : IRepository;
        void Rollback();
        void RollbackAndCloseConnection();
        T Repository<T>(string connectionString) where T : IRepository;
    }
}