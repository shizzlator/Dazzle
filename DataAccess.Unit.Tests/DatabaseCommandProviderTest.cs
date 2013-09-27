using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseCommandProviderTest
    {
        private Mock<IDatabaseConnectionProvider> _connectionProvider;
        private Mock<IDbConnection> _connection;
        private DatabaseCommandProvider _databaseCommandProvider;
        private Mock<ITransactionManager> _transactionManager;

        [SetUp]
        public void SetUp()
        {
            _connectionProvider = new Mock<IDatabaseConnectionProvider>();
            _connection = new Mock<IDbConnection>();
            _connectionProvider.Setup(x => x.GetOpenConnection()).Returns(_connection.Object);
            _transactionManager = new Mock<ITransactionManager>();
            _databaseCommandProvider = new DatabaseCommandProvider(_connectionProvider.Object, _transactionManager.Object);
        }

        [Test]
        public void ShouldCreateNewCommandForConnection()
        {
            //When
            _databaseCommandProvider.CreateCommandForCurrentConnection();

            //Then
            _connectionProvider.Verify(x => x.GetOpenConnection(), Times.Once());
        }
    }
}