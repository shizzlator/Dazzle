using System.Data;
using DataAccess.Command;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Tests.Unit
{
    [TestFixture]
    public class DatabaseCommandProviderTest
    {
        private Mock<IDatabaseConnectionProvider> _connectionProvider;
        private Mock<IDbConnection> _connection;
        private DatabaseCommandProvider _databaseCommandInstaceProvider;
        private Mock<ITransactionWrapper> _transactionWrapperMock;

        [SetUp]
        public void SetUp()
        {
            _connectionProvider = new Mock<IDatabaseConnectionProvider>();
            _connection = new Mock<IDbConnection>();
            _connectionProvider.Setup(x => x.GetOpenConnection()).Returns(_connection.Object);
            _transactionWrapperMock = new Mock<ITransactionWrapper>();
            _databaseCommandInstaceProvider = new DatabaseCommandProvider(_connectionProvider.Object, _transactionWrapperMock.Object);
        }

        [Test]
        public void ShouldCreateNewCommandForConnection()
        {
            //When
            _databaseCommandInstaceProvider.CreateCommandForCurrentConnection();

            //Then
            _connectionProvider.Verify(x => x.GetOpenConnection(), Times.Once());
        }
    }
}