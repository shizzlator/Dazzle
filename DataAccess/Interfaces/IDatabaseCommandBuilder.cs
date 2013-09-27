using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseCommandBuilder
    {
        IDbCommand CreateCommandFor(IDataQuery dataQuery);
    }
}