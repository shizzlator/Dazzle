namespace DataAccess.Interfaces
{
    public interface IObjectContainer
    {
        T GetInstanceOf<T>();
    }
}