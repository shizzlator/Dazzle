﻿using System;
using System.Data;
using DataAccess.Interfaces;
using DataAccess.Query;
using DataAccess.Session;
using Moq;
using NUnit.Framework;

namespace DataAccess.Tests.Unit
{
    [TestFixture]
    public class DatabaseSessionTest
    {
        private DataQuery _sqlDataQuery;
        private Mock<IDatabaseCommandFactory> _databaseCommandFactory;
        private Mock<IDbCommand> _command;
        private DatabaseSession _databaseSession;
        private Mock<ITransactionWrapper> _transactionWrapperMock;
        private Mock<IDatabaseReaderFactory> _databaseReaderFactory;
        private Mock<IConnectionHandler> _connectionHandler;

        [SetUp]
        public void BeforeEachTest()
        {
            _sqlDataQuery = new DataQuery();
            _command = new Mock<IDbCommand>();
            _databaseCommandFactory = new Mock<IDatabaseCommandFactory>();
            _transactionWrapperMock = new Mock<ITransactionWrapper>();
            _databaseReaderFactory = new Mock<IDatabaseReaderFactory>();
            _connectionHandler = new Mock<IConnectionHandler>();

            _databaseSession = new DatabaseSession(_databaseCommandFactory.Object, _transactionWrapperMock.Object, _databaseReaderFactory.Object, _connectionHandler.Object);

            _connectionHandler.Setup(x => x.GetHandler(It.IsAny<IDbCommand>(), It.IsAny<ITransactionWrapper>())).Returns(_connectionHandler.Object);
            _databaseCommandFactory.Setup(x => x.CreateCommandFor(It.IsAny<DataQuery>())).Returns(_command.Object);
        }

        [Test]
        public void ShouldExecuteScalarCommand()
        {
            //Act
            _databaseSession.ExecuteScalar(_sqlDataQuery);

            //Assert
            _databaseCommandFactory.Verify(x => x.CreateCommandFor(_sqlDataQuery), Times.Once());
            _command.Verify(x => x.ExecuteScalar(), Times.Once());
            _connectionHandler.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(TestException))]
        public void ShouldCleanUpWhenExceptionOccursDuringExecuteScalar()
        {
            //Arrange
            _command.Setup(x => x.ExecuteScalar()).Throws(new TestException());

            //Act
            _databaseSession.ExecuteScalar(_sqlDataQuery);
            
            //Assert
            _connectionHandler.Verify(x => x.RollbackTransactionAndCloseConnection(), Times.Once());
        }

        [Test]
        public void ShouldExecuteUpdateCommand()
        {
            //Act
            _databaseSession.ExecuteUpdate(_sqlDataQuery);

            //Assert
            _databaseCommandFactory.Verify(x => x.CreateCommandFor(_sqlDataQuery), Times.Once());
            _command.Verify(x => x.ExecuteNonQuery(), Times.Once());
            _connectionHandler.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(TestException))]
        public void ShouldCleanUpWhenExceptionOccursDuringExecuteUpdate()
        {
            //Arrange
            _command.Setup(x => x.ExecuteNonQuery()).Throws(new TestException());

            //Act
            _databaseSession.ExecuteUpdate(_sqlDataQuery);

            //Assert
            _connectionHandler.Verify(x => x.RollbackTransactionAndCloseConnection(), Times.Once());
        }

        [Test]
        public void ShouldExecuteReaderCommandInjectingCloseConnectionBehavior()
        {
            //Arrange
            _transactionWrapperMock.Setup(x => x.TransactionInProgress).Returns(false);
            var reader = new Mock<IDataReader>();
            _command.Setup(x => x.ExecuteReader(CommandBehavior.CloseConnection)).Returns(reader.Object);

            //Act
            _databaseSession.ExecuteReader(_sqlDataQuery);

            //Assert
            _command.Verify(x => x.ExecuteReader(CommandBehavior.CloseConnection), Times.Once());
            _databaseReaderFactory.Verify(x => x.CreateDataReader(reader.Object), Times.Once());
        }

        [Test]
        public void ShouldExecuteReaderCommandKeepingConnectionOpen()
        {
            //Arrange
            _transactionWrapperMock.Setup(x => x.TransactionInProgress).Returns(true);
            var reader = new Mock<IDataReader>();
            _command.Setup(x => x.ExecuteReader()).Returns(reader.Object);

            //Act
            _databaseSession.ExecuteReader(_sqlDataQuery);

            //Assert
            _command.Verify(x => x.ExecuteReader(CommandBehavior.CloseConnection), Times.Never());
            _command.Verify(x => x.ExecuteReader(), Times.Once());
            _databaseReaderFactory.Verify(x => x.CreateDataReader(reader.Object), Times.Once());
        }

        [Test]
        [ExpectedException(typeof(TestException))]
        public void ShouldCleanUpWhenExceptionOccursDuringExecuteReader()
        {
            //Arrange
            _databaseReaderFactory.Setup(x => x.CreateDataReader(It.IsAny<IDataReader>())).Throws(new TestException());

            //Act
            _databaseSession.ExecuteReader(_sqlDataQuery);

            //Assert
            _connectionHandler.Verify(x => x.RollbackTransactionAndCloseConnection(), Times.Once());
        }

        [Test]
        public void ShouldBeginTransaction()
        {
            //Act
            _databaseSession.BeginTransaction();

            //Assert
            _transactionWrapperMock.Verify(x => x.Begin(), Times.Once());
        }

        [Test]
        public void ShouldRollbackTransaction()
        {
            //Act
            _databaseSession.RollbackTransaction();

            //Assert
            _transactionWrapperMock.Verify(x => x.Rollback(), Times.Once());
        }

        [Test]
        public void ShouldCommitTransaction()
        {
            //Act
            _databaseSession.CommitTransaction();

            //Assert
            _transactionWrapperMock.Verify(x => x.Commit(), Times.Once());
        }
    }

    internal class TestException : Exception
    {
        public override string Message
        {
            get { return "This is a test exception intentionally thrown for unit testing"; }
        }
    }
}