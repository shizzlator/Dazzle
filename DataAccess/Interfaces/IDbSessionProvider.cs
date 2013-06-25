namespace DataAccess.Interfaces
{
    public interface IDbSessionProvider
    {
        IDatabaseSession CurrentDatabaseSession { get; }
    }
}