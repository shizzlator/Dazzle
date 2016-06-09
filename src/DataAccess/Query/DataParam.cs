using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Query
{
    public class DataParam : IDataParam
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterDirection? Direction { get; set; }
        public int? Size { get; set; }
        public DbType? Type { get; set; }
    }
}