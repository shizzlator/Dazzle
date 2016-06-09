using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Stubs
{
    public class DataQueryStub : IDataQuery
    {
        public string StoredProcName { get; set; }
        public string QueryText { get; set; }
        public string CommandText { get; set; }

        public IList<IDataParam> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public CommandType CommandType { get; set; }
        public TimeSpan? CommandTimeout { get; set; }
        private readonly IList<IDataParam> _parameters = new List<IDataParam>();

        #region Public Methods
        public IDataQuery WithCommandType(CommandType commandType)
        {
            CommandType = commandType;
            return this;
        }

        public IDataQuery WithParam(string name, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            _parameters.Add(new DataParamStub(value){Direction = direction, Name = name});
            return this;
        }

        public IDataQuery WithParam(string name, object value, DbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            _parameters.Add(new DataParamStub(value) { Direction = direction, Name = name });
            return this;
        }

        public IDataQuery WithQueryText(string queryText)
        {
            QueryText = queryText;
            return this;
        }

        public IDataQuery WithStoredProc(string storedProcName)
        {
            StoredProcName = storedProcName;
            return this;
        }

        public IDataQuery WithCommandTimeout(TimeSpan timeout)
        {
            CommandTimeout = timeout;
            return this;
        }

        #endregion
    }
}
