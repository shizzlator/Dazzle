using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection SqlConnection(string connectionString);
    }
}