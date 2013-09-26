using System.Collections.Generic;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDataQuery
    {
        string CommandText { get; set; }
        IDictionary<string, IDataParam> Parameters { get; set; }
        CommandType CommandType { get; }
        DataQuery WithParam(string name, object value);
        DataQuery WithQueryText(string queryText);
        DataQuery WithStoredProc(string storedProcName);
        DataQuery WithCommandType(CommandType commandType);
    }
}