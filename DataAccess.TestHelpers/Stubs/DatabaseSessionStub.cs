using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.TestHelpers.Stubs
{
    public class DatabaseSessionStub : IDatabaseSession
    {
        public List<DataQueryStub> Queries = new List<DataQueryStub>();
        public DataReaderStub DataReader = new DataReaderStub();


        #region Public Methods
        public IDataQuery CreateQuery()
        {
            DataQueryStub query = new DataQueryStub();
            Queries.Add(query);
            return query;
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new NotImplementedException();
        }

        public IDatabaseReader ExecuteReader(IDataQuery dataQuery)
        {
            return DataReader;
        }

        public object ExecuteScalar(IDataQuery dataQuery)
        {
            throw new NotImplementedException();
        }

        public IDataParameter ExecuteUpdate(IDataQuery dataQuery, string outputDataParameter)
        {
            throw new NotImplementedException();
        }

        public int ExecuteUpdate(IDataQuery dataQuery)
        {
            throw new NotImplementedException();
        }

        public void RollbackTransaction()
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Dispose()
        {
            
        }
    }
}


