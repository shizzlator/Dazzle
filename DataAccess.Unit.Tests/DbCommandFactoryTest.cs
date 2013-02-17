using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DbCommandFactoryTest
    {
        private Mock<IDatabaseConnectionManager> _connectionManager;
        private DatabaseCommandFactory _databaseCommandFactory;
        private DataQuery _dataQuery;
        private Mock<IDbCommand> _dbCommand;
        private Mock<IDbDataParameter> _dataParameter;
        private Mock<IDataParameterCollection> _dataParmeterCollection;
        private string _expectedCommandText = "command text";

        [SetUp]
        public void SetUp()
        {
            _connectionManager = new Mock<IDatabaseConnectionManager>();
            _databaseCommandFactory = new DatabaseCommandFactory(_connectionManager.Object);
            _dataQuery = new DataQuery() { CommandText = _expectedCommandText, CommandType = CommandType.Text };
            _dbCommand = new Mock<IDbCommand>();
            _dataParameter = new Mock<IDbDataParameter>();
            _dataParmeterCollection = new Mock<IDataParameterCollection>();

            _connectionManager.Setup(x => x.CreateCommandForCurrentConnection()).Returns(_dbCommand.Object);
            _dbCommand.Setup(x => x.CreateParameter()).Returns(_dataParameter.Object);
            _dbCommand.Setup(x => x.Parameters).Returns(_dataParmeterCollection.Object);
        }

        [Test]
        public void ShouldObtainAFreshCommandForTheCurrentConnection()
        {
            //When
            var command = _databaseCommandFactory.CreateCommandFor(_dataQuery);
    
            //Then
            _connectionManager.Verify(x => x.CreateCommandForCurrentConnection(), Times.Once());
            Assert.That(command, Is.EqualTo(_dbCommand.Object));
        }

        [Test]
        public void ShouldInitaliseCommandProperties()
        {
            //When
            _databaseCommandFactory.CreateCommandFor(_dataQuery);


            //Then
            _dbCommand.VerifySet(x => x.CommandText = _expectedCommandText);
            _dbCommand.VerifySet(x => x.CommandType = CommandType.Text);
        }

        [Test]
        public void ShouldInitialiseCommandDataParameters()
        {
            //Given
            _dataQuery.AddParam("Title", "Mr");
            _dataQuery.AddParam("FirstName", "David");
            _dataQuery.AddParam("Surname", "Miranda");

            //When
            _databaseCommandFactory.CreateCommandFor(_dataQuery);

            //Then
            _dataParameter.VerifySet(x => x.ParameterName = "Title", Times.Once());
            _dataParameter.VerifySet(x => x.Value = "Mr", Times.Once());
            _dataParameter.VerifySet(x => x.ParameterName = "FirstName", Times.Once());
            _dataParameter.VerifySet(x => x.Value = "David", Times.Once());
            _dataParameter.VerifySet(x => x.ParameterName = "Surname", Times.Once());
            _dataParameter.VerifySet(x => x.Value = "Miranda", Times.Once());
        }
    }
}