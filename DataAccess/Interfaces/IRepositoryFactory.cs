namespace DataAccess.Interfaces
{
    public interface IRepositoryFactory
    {
        T GetInstanceOf<T>(IDatabaseSession databaseSession) where T : IRepository;
    }
}