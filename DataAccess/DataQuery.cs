using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DataQuery : IDataQuery
    {
        private IDictionary<string, object> _parameters = new Dictionary<string, object>();
        public string CommandText { get; set; }
        public IDictionary<string, object> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public CommandType CommandType { get; set; }

        public DataQuery AddParam(string name, object value)
        {
            _parameters.Add(name, value);
            return this;
        }

        public DataQuery WithText(string name)
        {
            CommandText = name;
            return this;
        }

        public DataQuery WithCommandType(CommandType commandType)
        {
            CommandType = commandType;
            return this;
        }
    }
}