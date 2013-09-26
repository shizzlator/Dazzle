using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseConnectionManager
    {
        IDbCommand CreateCommandForCurrentConnection();
    }
}
