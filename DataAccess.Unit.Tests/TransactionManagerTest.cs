using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class TransactionManagerTest
    {
        private Mock<IDbTransaction> _transientTransaction;
        private Mock<IDatabaseConnectionProvider> _databaseConnectionProvider;
        private TransactionManager _transactionManager;
        private Mock<IDbConnection> _connection;

        [SetUp]
        public void SetUp()
        {
            _databaseConnectionProvider = new Mock<IDatabaseConnectionProvider>();
            _transientTransaction = new Mock<IDbTransaction>();
            _connection = new Mock<IDbConnection>();
            _transientTransaction.Setup(x => x.Connection).Returns(_connection.Object);
            _databaseConnectionProvider.Setup(x => x.GetOpenConnection().BeginTransaction()).Returns(_transientTransaction.Object);
            _transactionManager = new TransactionManager(_databaseConnectionProvider.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionManager.RollbackDispose();
        }

        [Test]
        public void ShouldCreateTransientTransaction()
        {
            //When
            _transactionManager.Begin();

            //Then
            Assert.That(new TransactionManager(_databaseConnectionProvider.Object).TransientTransaction, Is.EqualTo(_transientTransaction.Object));
        }

        [Test]
        public void ShouldBeginTransaction()
        {
            //When
            _transactionManager.Begin();

            //Then
            _databaseConnectionProvider.Verify(x => x.GetOpenConnection().BeginTransaction(), Times.Once());
        }

        [Test]
        public void ShouldCommitTransactionWhenOneIsInProgress()
        {
            //Given
            _transactionManager.Begin();

            //When
            _transactionManager.Commit();

            //Then
            _transientTransaction.Verify(x => x.Commit(), Times.Once());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldRollbackTransactionWhenOneIsInProgress()
        {
            //Given
            _transactionManager.Begin();

            //When
            _transactionManager.Rollback();

            //Then
            _transientTransaction.Verify(x => x.Rollback(), Times.Once());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldOnlyBeginTheUnderlyingTransactionOnce()
        {
            _transactionManager.Begin();
            _transactionManager.Begin();
            _transactionManager.Begin();

            _databaseConnectionProvider.Verify(x => x.GetOpenConnection().BeginTransaction(), Times.Once());
        }

        [Test]
        public void ShouldRollbackAndDisposeTransaction()
        {
            //Given
            _transactionManager.Begin();

            //When
            _transactionManager.RollbackDispose();

            //Then
            _transientTransaction.Verify(x => x.Rollback(), Times.Once());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
            _connection.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldNotRollbackTransactionWhenNoTransactionIsInProgress()
        {
            //Given
            _transactionManager.Begin();
            _transactionManager.Commit();

            //When
            _transactionManager.RollbackDispose();

            //Then
            _transientTransaction.Verify(x => x.Rollback(), Times.Never());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
        }
    }
}