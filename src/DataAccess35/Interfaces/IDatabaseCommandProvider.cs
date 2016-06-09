using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseCommandProvider
    {
        IDbCommand CreateCommandForCurrentConnection();
    }
}
