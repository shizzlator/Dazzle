namespace DataAccess.Interfaces
{
    public interface IRepositoryFactory
    {
        T GetInstanceOf<T>() where T : IRepository;
        T GetInstanceOf<T>(string connectionString) where T : IRepository;
        T GetInstanceOf<T>(IDatabaseSession databaseSession) where T : IRepository;
    }
}