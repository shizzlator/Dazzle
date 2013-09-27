using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.DataQuery
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

        public DataQuery WithParam(string name, object value)
        {
            _parameters.Add(name, new DataParam { Value = value });
            return this;
        }

        public DataQuery WithQueryText(string queryText)
        {
            CommandType = CommandType.Text;
            CommandText = queryText;
            return this;
        }

        public DataQuery WithStoredProc(string storedProcName)
        {
            CommandType = CommandType.StoredProcedure;
            CommandText = storedProcName;
            return this;
        }

        public DataQuery WithCommandType(CommandType commandType)
        {
            CommandType = commandType;
            return this;
        }
    }
}