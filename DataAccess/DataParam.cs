using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DataParam : IDataParam
    {
        public object Value { get; set; }
        public ParameterDirection? Direction { get; set; }
        public int? Size { get; set; }
        public SqlDbType? Type { get; set; }
    }
}