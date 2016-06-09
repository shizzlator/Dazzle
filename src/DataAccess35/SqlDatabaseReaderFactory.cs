using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    internal class SqlDatabaseReaderFactory : IDatabaseReaderFactory
    {
        public IDatabaseReader CreateDataReader(IDataReader dataReader)
        {
            return new SqlDatabaseReader(dataReader);
        }
    }
}