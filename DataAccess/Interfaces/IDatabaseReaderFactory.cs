using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseReaderFactory
    {
        IDatabaseReader CreateDataReader(IDataReader dataReader);
    }

    public class SqlDatabaseReaderFactory : IDatabaseReaderFactory
    {
        public IDatabaseReader CreateDataReader(IDataReader dataReader)
        {
            return new SqlDatabaseReader(dataReader);
        }
    }
}