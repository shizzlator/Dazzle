namespace DataAccess.Interfaces
{
    public interface IRepositoryContainer
    {
        T GetInstanceOf<T>();
    }
}