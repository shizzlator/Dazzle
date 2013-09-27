using System;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseSession
    {
        IDataQuery CreateQuery();
        object RunScalarCommandFor(IDataQuery dataQuery);
        int RunUpdateCommandFor(IDataQuery dataQuery);
        IDataReader RunReaderFor(IDataQuery dataQuery);
        IDataParameter RunUpdateCommandFor(IDataQuery dataQuery, string outputDataParameter);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
