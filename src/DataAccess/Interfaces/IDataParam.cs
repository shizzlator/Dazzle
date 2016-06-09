using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDataParam
    {
        string Name { get; set; }
        object Value { get; set; }
        ParameterDirection? Direction { get; set; }
        int? Size { get; set; }
        DbType? Type { get; set; }
    }
}