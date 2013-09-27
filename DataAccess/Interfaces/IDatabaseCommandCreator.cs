using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseCommandCreator
    {
        IDbCommand CreateCommandFor(IDataQuery dataQuery);
    }
}