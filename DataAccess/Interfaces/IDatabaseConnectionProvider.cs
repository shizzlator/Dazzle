using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseConnectionProvider
    {
        IDbConnection GetOpenConnection();
    }
}