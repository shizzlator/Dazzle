using System.Data;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseReaderFactoryTest
    {
        [Test]
        public void ShouldGetInstanceOfSqlDataReader()
        {
            var dataReader = new Mock<IDataReader>().Object;
            var sqlDatabaseReader = new SqlDatabaseReaderFactory().CreateDataReader(dataReader);

            Assert.IsInstanceOf<SqlDatabaseReader>(sqlDatabaseReader);
        }
    }
}