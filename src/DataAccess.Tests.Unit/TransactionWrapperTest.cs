using System;
using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Tests.Unit
{
    [TestFixture]
    public class TransactionWrapperTest
    {
        private Mock<IDbTransaction> _transientTransaction;
        private Mock<IDatabaseConnectionProvider> _databaseConnectionProvider;
        private TransactionWrapper _transactionWrapper;
        private Mock<IDbConnection> _connection;

        [SetUp]
        public void SetUp()
        {
            _databaseConnectionProvider = new Mock<IDatabaseConnectionProvider>();
            _transientTransaction = new Mock<IDbTransaction>();
            _connection = new Mock<IDbConnection>();
            _databaseConnectionProvider.Setup(x => x.GetOpenConnection()).Returns(_connection.Object);
            _connection.Setup(x => x.BeginTransaction()).Returns(_transientTransaction.Object);
            _transactionWrapper = new TransactionWrapper(_databaseConnectionProvider.Object);

            //Reset the transient transaction
            _transactionWrapper.Rollback();
        }

        [Test]
        public void ShouldBeginTransaction()
        {
            //When
            _transactionWrapper.Begin();

            //Then
            _connection.Verify(x=> x.BeginTransaction(), Times.Once());
        }

        [Test]
        public void ShouldCommitTransactionWhenOneIsInProgress()
        {
            //Given
            _transactionWrapper.Begin();

            //When
            _transactionWrapper.Commit();

            //Then
            _transientTransaction.Verify(x => x.Commit(), Times.Once());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "commit failed")]
        public void ShouldRollbackTransactionWhenCommitFails()
        {
            //Given
            _transactionWrapper.Begin();
            _transientTransaction.Setup(x => x.Commit()).Throws(new Exception("commit failed"));

            //When
            _transactionWrapper.Commit();

            //Then
            _transientTransaction.Verify(x => x.Commit(), Times.Once());
            _transientTransaction.Verify(x => x.Rollback(), Times.Once());
            _connection.Verify(x => x.Dispose(), Times.Never());
        }

        [Test]
        public void ShouldRollbackTransactionWhenOneIsInProgress()
        {
            //Given
            _transactionWrapper.Begin();

            //When
            _transactionWrapper.Rollback();

            //Then
            _transientTransaction.Verify(x => x.Rollback(), Times.Once());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldOnlyBeginTheTransientTransactionOnce()
        {
            _transactionWrapper.Begin();
            _transactionWrapper.Begin();
            _transactionWrapper.Begin();

            _connection.Verify(x => x.BeginTransaction(), Times.Once());
        }

        [Test]
        public void ShouldNotRollbackTransactionWhenNoTransactionIsInProgress()
        {
            //Given
            _transactionWrapper.Begin();
            _transactionWrapper.Commit();

            //When
            _transactionWrapper.Rollback();

            //Then
            _transientTransaction.Verify(x => x.Rollback(), Times.Never());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
        }
    }
}