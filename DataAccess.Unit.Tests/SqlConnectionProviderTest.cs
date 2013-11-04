using System.Data;
using DataAccess.Connection;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class SqlConnectionProviderTest
    {
        private Mock<IDatabaseConnectionFactory> _fakeConectionFactory;
        private string _connectionString;
        private SqlConnectionProvider _sqlConnectionProvider;
        private Mock<IDbConnection> _fakeConnection;

        [SetUp]
        public void SetUp()
        {
            _fakeConectionFactory = new Mock<IDatabaseConnectionFactory>();
            _connectionString = "shizzleSticks";
            _sqlConnectionProvider = new SqlConnectionProvider(_connectionString, _fakeConectionFactory.Object);
            _fakeConnection = new Mock<IDbConnection>();
            _fakeConectionFactory.Setup(x => x.SqlConnection(_connectionString)).Returns(_fakeConnection.Object);
        }

        [Test]
        public void ShouldOnlyInitialiseConnectionOnce()
        {
            _sqlConnectionProvider.GetOpenConnection();
            _fakeConnection.Setup(x => x.ConnectionString).Returns(_connectionString);

            _sqlConnectionProvider.GetOpenConnection();

            _fakeConectionFactory.Verify(x => x.SqlConnection(_connectionString), Times.Once());
        }

        [Test]
        public void ShouldOpenConnection()
        {
            _fakeConnection.Setup(x => x.State).Returns(ConnectionState.Closed);

            _sqlConnectionProvider.GetOpenConnection();

            _fakeConnection.Verify(x => x.Open(), Times.Once());
        }

        [Test]
        public void ShouldNotOpenConnectionWhenAlreadyOpen()
        {
            _fakeConnection.Setup(x => x.State).Returns(ConnectionState.Open);

            _sqlConnectionProvider.GetOpenConnection();

            _fakeConnection.Verify(x => x.Open(), Times.Never());
        }
        
    }
}