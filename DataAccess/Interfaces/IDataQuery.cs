using System.Collections.Generic;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDataQuery
    {
        string CommandText { get; set; }
        IDictionary<string, IDataParam> Parameters { get; set; }
        CommandType CommandType { get; }
        DataQuery.DataQuery WithParam(string name, object value);
        DataQuery.DataQuery WithQueryText(string queryText);
        DataQuery.DataQuery WithStoredProc(string storedProcName);
        DataQuery.DataQuery WithCommandType(CommandType commandType);
    }
}