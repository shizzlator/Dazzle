﻿using System;
using System.Data;
using System.Threading;
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
            _databaseConnectionProvider.Setup(x => x.GetOpenConnection()).Returns(_connection.Object);
            _connection.Setup(x => x.BeginTransaction()).Returns(_transientTransaction.Object);
            _transactionManager = new TransactionManager(_databaseConnectionProvider.Object);

            //Reset the transient transaction
            _transactionManager.Rollback();
        }

        [Test]
        public void ShouldBeginTransaction()
        {
            //When
            _transactionManager.Begin();

            //Then
            _connection.Verify(x=> x.BeginTransaction(), Times.Once());
        }

        [Test]
        public void ShouldBeginTransientTransaction()
        {
            //When
            _transactionManager.Begin();

            //Then
            Assert.That(new TransactionManager(_databaseConnectionProvider.Object).TransientTransaction, Is.EqualTo(_transientTransaction.Object));
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
        [ExpectedException(typeof(Exception), ExpectedMessage = "commit failed")]
        public void ShouldRollbackTransactionWhenCommitFails()
        {
            //Given
            _transactionManager.Begin();
            _transientTransaction.Setup(x => x.Commit()).Throws(new Exception("commit failed"));

            //When
            _transactionManager.Commit();

            //Then
            _transientTransaction.Verify(x => x.Commit(), Times.Once());
            _transientTransaction.Verify(x => x.Rollback(), Times.Once());
            _connection.Verify(x => x.Dispose(), Times.Never());
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
        public void ShouldOnlyBeginTheTransientTransactionOnce()
        {
            _transactionManager.Begin();
            _transactionManager.Begin();
            _transactionManager.Begin();

            _connection.Verify(x => x.BeginTransaction(), Times.Once());
        }

        [Test]
        public void ShouldNotRollbackTransactionWhenNoTransactionIsInProgress()
        {
            //Given
            _transactionManager.Begin();
            _transactionManager.Commit();

            //When
            _transactionManager.Rollback();

            //Then
            _transientTransaction.Verify(x => x.Rollback(), Times.Never());
            _transientTransaction.Verify(x => x.Dispose(), Times.Once());
        }
    }
}