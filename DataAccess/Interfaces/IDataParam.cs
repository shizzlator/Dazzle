using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDataParam
    {
        object Value { get; set; }
        ParameterDirection? Direction { get; set; }
        int? Size { get; set; }
        SqlDbType? Type { get; set; }
    }
}