using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseSessionTest
    {
        private DataQuery _dataQuery;
        private Mock<IDatabaseCommandFactory> _commandFactory;
        private Mock<IDbCommand> _command;
        private DatabaseSession _databaseSession;
        private Mock<ITransactionManager> _transactionManager;
        private Mock<IDbConnection> _connection;

        [SetUp]
        public void BeforeEachTest()
        {
            _dataQuery = new DataQuery();
            _commandFactory = new Mock<IDatabaseCommandFactory>();
            _command = new Mock<IDbCommand>();
            _transactionManager = new Mock<ITransactionManager>();
            _databaseSession = new DatabaseSession(_commandFactory.Object, _transactionManager.Object);
            _connection = new Mock<IDbConnection>();
            _command.Setup(x => x.Connection).Returns(_connection.Object);
            _commandFactory.Setup(x => x.CreateCommandFor(It.IsAny<DataQuery>())).Returns(_command.Object);
        }

        [Test]
        public void ShouldCreateRunAndDisposeOfScalarCommandAndConnectionForDataQueryWithNoTransaction()
        {
            //When
            _databaseSession.RunScalarCommandFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteScalar(), Times.Once());
            _command.Verify(x => x.Dispose(), Times.Once());
            _connection.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void ShouldCreateRunAndDisposeOfUpdateCommandAndConnectionForDataQueryWithNoTransaction()
        {
            //When
            _databaseSession.RunUpdateCommandFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteNonQuery(), Times.Once());
            _command.Verify(x => x.Dispose(), Times.Once());
            _connection.Verify(x => x.Close(), Times.Once());
        }

        [Test]
        public void ShouldCreateReturnAndDisposeOfReaderCommandAndConnectionForDataQueryWithNoTransaction()
        {
            //Given
            var dataReader = new Mock<IDataReader>();
            _command.Setup(x => x.ExecuteReader(CommandBehavior.CloseConnection)).Returns(dataReader.Object);
            
            //When
            var reader = _databaseSession.RunReaderFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteReader(CommandBehavior.CloseConnection), Times.Once());
            Assert.That(reader, Is.EqualTo(dataReader.Object));
            _command.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldCreateRunAndDisposeOfScalarCommandForDataQueryWithTransaction()
        {
            //Given
            _transactionManager.Setup(x => x.TransactionInProgress).Returns(true);

            //When
            _databaseSession.RunScalarCommandFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteScalar(), Times.Once());
            _command.Verify(x => x.Dispose(), Times.Once());
            _connection.Verify(x => x.Close(), Times.Never());
        }

        [Test]
        public void ShouldCreateRunAndDisposeOfUpdateCommandForDataQueryWithTransaction()
        {
            //Given
            _transactionManager.Setup(x => x.TransactionInProgress).Returns(true);

            //When
            _databaseSession.RunUpdateCommandFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteNonQuery(), Times.Once());
            _command.Verify(x => x.Dispose(), Times.Once());
            _connection.Verify(x => x.Close(), Times.Never());
        }

        [Test]
        public void ShouldCreateReturnAndDisposeOfReaderCommandForDataQueryWithTransaction()
        {
            //Given
            _transactionManager.Setup(x => x.TransactionInProgress).Returns(true);
            var dataReader = new Mock<IDataReader>();
            _command.Setup(x => x.ExecuteReader()).Returns(dataReader.Object);

            //When
            var reader = _databaseSession.RunReaderFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteReader(), Times.Once());
            _command.Verify(x => x.ExecuteReader(CommandBehavior.CloseConnection), Times.Never());
            Assert.That(reader, Is.EqualTo(dataReader.Object));
            _command.Verify(x => x.Dispose(), Times.Once());
        }
    }
}