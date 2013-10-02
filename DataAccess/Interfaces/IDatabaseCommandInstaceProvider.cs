using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseCommandInstaceProvider
    {
        IDbCommand CreateCommandForCurrentConnection();
    }
}
