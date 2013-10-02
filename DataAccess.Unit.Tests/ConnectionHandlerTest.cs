using System.Data;
using DataAccess.Connection;
using DataAccess.Interfaces;
using DataAccess.Session;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class ConnectionHandlerTest
    {
        private Mock<ITransactionManager> _transactionManager;
        private Mock<IDbCommand> _dbCommand;
        private Mock<IDbConnection> _connection;

        [SetUp]
        public void SetUp()
        {
            _transactionManager = new Mock<ITransactionManager>();
            _dbCommand = new Mock<IDbCommand>();
            _connection = new Mock<IDbConnection>();
            _dbCommand.Setup(x => x.Connection).Returns(_connection.Object);
        }

        [Test]
        public void ShouldInitialiseConnectionFromCommand()
        {
            new ConnectionHandler().GetHandler(_dbCommand.Object, _transactionManager.Object);

            _dbCommand.VerifyGet(x => x.Connection);
        }

        [Test]
        public void ShouldCloseConnectionOnDispose()
        {
            _connection.Setup(x => x.State).Returns(ConnectionState.Open);

            using (new ConnectionHandler().GetHandler(_dbCommand.Object, _transactionManager.Object))
            {
            }

            _connection.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void ShouldNotCloseClosedConnectionOnDispose()
        {
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            using (new ConnectionHandler().GetHandler(_dbCommand.Object, _transactionManager.Object))
            {
            }

            _connection.Verify(x => x.Close(), Times.Never());
        }

        [Test]
        public void ShouldRollbackTransaction()
        {
            _transactionManager.Setup(x => x.TransactionInProgress).Returns(true);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommand.Object, _transactionManager.Object);

            connectionHandler.CleanUp();
            
            _transactionManager.Verify(x => x.Rollback(), Times.Once());
        }

        [Test]
        public void ShouldNotRollbackTransactionWhenNoTransactionIsInProgress()
        {
            _transactionManager.Setup(x => x.TransactionInProgress).Returns(false);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommand.Object, _transactionManager.Object);

            connectionHandler.CleanUp();

            _transactionManager.Verify(x => x.Rollback(), Times.Never());
        }

        [Test]
        public void ShouldCloseConnectionDuringCleanUp()
        {
            _connection.Setup(x => x.State).Returns(ConnectionState.Open);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommand.Object, _transactionManager.Object);

            connectionHandler.CleanUp();

            _connection.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void ShouldNotCloseClosedConnectionDuringCleanUp()
        {
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);
            var connectionHandler = new ConnectionHandler().GetHandler(_dbCommand.Object, _transactionManager.Object);

            connectionHandler.CleanUp();

            _connection.Verify(x => x.Close(), Times.Never());
        }
    }
}