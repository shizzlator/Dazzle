using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDataQuery
    {
        string CommandText { get; set; }
        IDictionary<string, IDataParam> Parameters { get; }
        CommandType CommandType { get; }
        TimeSpan? CommandTimeout { get; }

        IDataQuery WithParam(string name, object value, ParameterDirection direction = ParameterDirection.Input);
        IDataQuery WithQueryText(string queryText);
        IDataQuery WithStoredProc(string storedProcName);
        IDataQuery WithCommandType(CommandType commandType);
        IDataQuery WithCommandTimeout(TimeSpan timeout);
    }
}