using System.Data;
using System.Data.SqlClient;
using DataAccess.Interfaces;

namespace DataAccess.Connection
{
    internal class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public IDbConnection SqlConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}