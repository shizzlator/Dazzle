using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Query
{
    public class DataQuery : IDataQuery
    {
        private IDictionary<string, IDataParam> _parameters = new Dictionary<string, IDataParam>();
        public string CommandText { get; set; }
        public IDictionary<string, IDataParam> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public CommandType CommandType { get; private set; }
        public TimeSpan? CommandTimeout { get; private set; }


        public IDataQuery WithParam(string name, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            _parameters.Add(name, new DataParam
            {
                Value = value,
                Direction = direction
            });
            return this;
        }

        public IDataQuery WithQueryText(string queryText)
        {
            CommandType = CommandType.Text;
            CommandText = queryText;
            return this;
        }

        public IDataQuery WithStoredProc(string storedProcName)
        {
            CommandType = CommandType.StoredProcedure;
            CommandText = storedProcName;
            return this;
        }

        public IDataQuery WithCommandType(CommandType commandType)
        {
            CommandType = commandType;
            return this;
        }

        public IDataQuery WithCommandTimeout(TimeSpan timeout)
        {
            CommandTimeout = timeout;
            return this;
        }
    }
}