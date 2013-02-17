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

        [SetUp]
        public void BeforeEachTest()
        {
            _dataQuery = new DataQuery();
            _commandFactory = new Mock<IDatabaseCommandFactory>();
            _command = new Mock<IDbCommand>();
            _transactionManager = new Mock<ITransactionManager>();
            _databaseSession = new DatabaseSession(_commandFactory.Object, _transactionManager.Object);
            _commandFactory.Setup(x => x.CreateCommandFor(It.IsAny<DataQuery>())).Returns(_command.Object);
        }

        [Test]
        public void ShouldCreateRunAndDisposeOfScalarCommandForDataQuery()
        {
            //When
            _databaseSession.RunScalarCommandFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteScalar(), Times.Once());
            _command.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldCreateRunAndDisposeOfUpdateCommandForDataQuery()
        {
            //When
            _databaseSession.RunUpdateCommandFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteNonQuery(), Times.Once());
            _command.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ShouldCreateReturnAndDisposeOfReaderCommandForDataQuery()
        {
            //Given
            var dataReader = new Mock<IDataReader>();
            _command.Setup(x => x.ExecuteReader()).Returns(dataReader.Object);
            
            //When
            var reader = _databaseSession.RunReaderFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteReader(), Times.Once());
            Assert.That(reader, Is.EqualTo(dataReader.Object));
            _command.Verify(x => x.Dispose(), Times.Once());
        }

        public void ShouldDisposeOfReaderWhenRedaerContainsNoRows()
        {
            //Given
            var dataReader = new Mock<IDataReader>();
            _command.Setup(x => x.ExecuteReader()).Returns(dataReader.Object);

            //When
            var reader = _databaseSession.RunReaderFor(_dataQuery);

            //Then
            _commandFactory.Verify(x => x.CreateCommandFor(_dataQuery), Times.Once());
            _command.Verify(x => x.ExecuteReader(), Times.Once());
            Assert.That(reader, Is.EqualTo(dataReader.Object));
            _command.Verify(x => x.Dispose(), Times.Once());
        }
    }
}