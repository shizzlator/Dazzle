using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.TestHelpers.Stubs
{
    public class DataQueryStub : IDataQuery
    {
        public string StoredProcName { get; set; }
        public string QueryText { get; set; }
        public string CommandText { get; set; }

        public CommandType CommandType { get; set; }

        private Dictionary<string, IDataParam> _parameters = new Dictionary<string, IDataParam>();
        public IDictionary<string, IDataParam> Parameters { get { return _parameters; } }


        #region Public Methods
        public IDataQuery WithCommandType(CommandType commandType)
        {
            CommandType = commandType;
            return this;
        }

        public IDataQuery WithParam(string name, object value)
        {
            _parameters.Add(name, new DataParamStub(value));
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
        #endregion
    }
}
