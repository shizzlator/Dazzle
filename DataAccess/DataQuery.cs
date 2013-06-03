using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
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

        public CommandType CommandType { get; set; }

        public DataQuery AddParam(string name, object value)
        {
            _parameters.Add(name, new DataParam { Value = value });
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