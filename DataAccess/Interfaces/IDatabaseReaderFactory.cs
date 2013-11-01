using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseReaderFactory
    {
        IDatabaseReader CreateDataReader(IDataReader dataReader);
    }
}