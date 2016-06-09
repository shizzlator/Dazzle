namespace DataAccess.Interfaces
{
    public interface IDatabaseSessionFactory
    {
        IDatabaseSession CreateSession(string connectionString);
        IDatabaseSession CreateSession();
    }
}
