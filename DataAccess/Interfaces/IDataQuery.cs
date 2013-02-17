using System.Collections.Generic;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDataQuery
    {
        string CommandText { get; set; }
        IDictionary<string, object> Parameters { get; set; }
        CommandType CommandType { get; set; }
    }
}