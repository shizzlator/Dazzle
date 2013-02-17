using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseConnectionManager
    {
        IDbConnection Connection { get; }
        IDbCommand CreateCommandForCurrentConnection();
    }
}
