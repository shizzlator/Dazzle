namespace DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        T Repository<T>() where T : IRepository;
        void Rollback();
        void Dispose();
        void RollbackAndCloseConnection();
    }
}