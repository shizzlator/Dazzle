using System.Data;
using DataAccess.Connection;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Tests.Unit
{
    [TestFixture]
    public class ConnectionHandlerTest
    {
        private Mock<ITransactionWrapper> _transactionWrapperMock;
        private Mock<IDbCommand> _dbCommandMock;
        private Mock<IDbConnection> _connectionMock;

        [SetUp]
        public void SetUp()
        {
            _transactionWrapperMock = new Mock<ITransactionWrapper>();
            _dbCommandMock = new Mock<IDbCommand>();
            _connectionMock = new Mock<IDbConnection>();
            _dbCommandMock.Setup(x => x.Connection).Returns(_connectionMock.Object);
        }

        [Test]
        public void ShouldInitialiseConnectionFromCommand()
        {
            new ConnectionHandler().GetHandler(_dbCommandMock.Object, _transactionWrapperMock.Object);

            _dbCommandMock.VerifyGet(x => x.Connection);
        }

        [TestCase(ConnectionState.Broken)]
        [TestCase(ConnectionState.Connecting)]
        [TestCase(ConnectionState.Executing)]
        [TestCase(ConnectionState.Fetching)]
        [TestCase(ConnectionState.Open)]
        public void ShouldCloseConnectionOnDispose(ConnectionState connectionState)
        {
            _connectionMock.Setup(x => x.State).Returns(connectionState);

            using (new ConnectionHandler().GetHandler(_dbCommandMock.Object, _transactionWrapperMock.Object))
            {
            }

            _connectionMock.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void ShouldNotCloseClosedConnectionOnDispose()
        {
            _connectionMock.Setup(x => x.State).Returns(ConnectionState.Closed);

            using (new ConnectionHandler().GetHandler(_dbCommandMock.Object, _transactionWrapperMock.Object))
            {
            }

            _connectionMock.Verify(x => x.Close(), Times.Never());
        }

        [Test]
        public void ShouldRollbackTransaction()
        {
            _transactionWrapperMock.Setup(x => x.TransactionInProgress).Returns(true);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommandMock.Object, _transactionWrapperMock.Object);

            connectionHandler.RollbackTransactionAndCloseConnection();
            
            _transactionWrapperMock.Verify(x => x.Rollback(), Times.Once());
        }

        [Test]
        public void ShouldNotRollbackTransactionWhenNoTransactionIsInProgress()
        {
            _transactionWrapperMock.Setup(x => x.TransactionInProgress).Returns(false);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommandMock.Object, _transactionWrapperMock.Object);

            connectionHandler.RollbackTransactionAndCloseConnection();

            _transactionWrapperMock.Verify(x => x.Rollback(), Times.Never());
        }

        [Test]
        public void ShouldCloseConnectionDuringCleanUp()
        {
            _connectionMock.Setup(x => x.State).Returns(ConnectionState.Open);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommandMock.Object, _transactionWrapperMock.Object);

            connectionHandler.RollbackTransactionAndCloseConnection();

            _connectionMock.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void ShouldNotCloseClosedConnectionDuringCleanUp()
        {
            _connectionMock.Setup(x => x.State).Returns(ConnectionState.Closed);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommandMock.Object, _transactionWrapperMock.Object);

            connectionHandler.RollbackTransactionAndCloseConnection();

            _connectionMock.Verify(x => x.Close(), Times.Never());
        }
    }
}