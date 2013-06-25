using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseConnetionManagerTest
    {
        private Mock<IDatabaseConnectionProvider> _connectionProvider;
        private Mock<IDbConnection> _connection;
        private DatabaseConnectionManager _databaseConnectionManager;
        private Mock<ITransactionManager> _transactionManager;

        [SetUp]
        public void SetUp()
        {
            _connectionProvider = new Mock<IDatabaseConnectionProvider>();
            _connection = new Mock<IDbConnection>();
            _connectionProvider.Setup(x => x.GetOpenConnection()).Returns(_connection.Object);
            _transactionManager = new Mock<ITransactionManager>();
            _databaseConnectionManager = new DatabaseConnectionManager(_connectionProvider.Object, _transactionManager.Object);
        }

        [Test]
        public void ShouldCreateNewCommandForConnection()
        {
            //When
            _databaseConnectionManager.CreateCommandForCurrentConnection();

            //Then
            _connectionProvider.Verify(x => x.GetOpenConnection(), Times.Once());
        }

        [Test]
        public void ShouldOpenConnection()
        {
            //When
            _databaseConnectionManager.CreateCommandForCurrentConnection();

            //Then
            _connectionProvider.Verify(x => x.GetOpenConnection(), Times.Once());
            _connection.Verify(x => x.Open(), Times.Once());
        }

        [Test]
        public void ShouldOnlyCreateAndOpenConnectionOnceForConsecutiveCalls()
        {
            //When
            _databaseConnectionManager.CreateCommandForCurrentConnection();
            _databaseConnectionManager.CreateCommandForCurrentConnection();

            //Then
            _connectionProvider.Verify(x => x.GetOpenConnection(), Times.Once());
            _connection.Verify(x => x.Open(), Times.Once());
        }

        [Test]
        public void ShouldInitialiseCommandWithTransientTransaction()
        {
            //Given
            var command = new Mock<IDbCommand>();
            _transactionManager.SetupGet(x => x.TransactionInProgress).Returns(true);
            var transaction = new Mock<IDbTransaction>();
            _transactionManager.SetupGet(x => x.TransientTransaction).Returns(transaction.Object);
            _connection.Setup(x => x.CreateCommand()).Returns(command.Object);

            //When
            _databaseConnectionManager.CreateCommandForCurrentConnection();

            //Then
            command.VerifySet(x => x.Transaction = transaction.Object);
            _connection.Verify(x => x.Open(), Times.Once());
        }

        [Test]
        public void ShouldOpenConnectionOnceForConsecutiveTransactionalCalls()
        {
            //Given
            var command = new Mock<IDbCommand>();
            var transaction = new Mock<IDbTransaction>();
            _transactionManager.SetupGet(x => x.TransactionInProgress).Returns(true);
            _transactionManager.SetupGet(x => x.TransientTransaction).Returns(transaction.Object);
            _connection.Setup(x => x.CreateCommand()).Returns(command.Object);

            //When
            _databaseConnectionManager.CreateCommandForCurrentConnection();
            _databaseConnectionManager.CreateCommandForCurrentConnection();

            //Then
            command.VerifySet(x => x.Transaction = transaction.Object);
            _connection.Verify(x => x.Open(), Times.Once());
        }

    }
}