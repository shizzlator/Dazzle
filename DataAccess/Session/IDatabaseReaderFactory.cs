using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Session
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