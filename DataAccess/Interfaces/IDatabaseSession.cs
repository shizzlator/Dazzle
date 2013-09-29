using System;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseSession
    {
        IDataQuery CreateQuery();
        object ExecuteScalar(IDataQuery dataQuery);
        int ExecuteUpdate(IDataQuery dataQuery);
        IDatabaseReader ExecuteReader(IDataQuery dataQuery);
        IDataParameter ExecuteUpdate(IDataQuery dataQuery, string outputDataParameter);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
