using System;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseSession
    {
        object RunScalarCommandFor(IDataQuery dataQuery);
        int RunUpdateCommandFor(IDataQuery dataQuery);
        IDataReader RunReaderFor(IDataQuery dataQuery);
    }
}
