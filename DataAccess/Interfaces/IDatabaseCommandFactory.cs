using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseCommandFactory
    {
        IDbCommand CreateCommandFor(IDataQuery dataQuery);
    }
}