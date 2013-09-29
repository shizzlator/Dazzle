using System.Data;
using DataAccess.Command;
using DataAccess.Interfaces;
using DataAccess.Query;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseCommandBuilderTest
    {
        private DatabaseCommandBuilder _databaseCommandBuilder;
        private Mock<IDatabaseCommandProvider> _connectionManager;
        private Mock<IDbCommand> _dbCommand;
        private Mock<IDbDataParameter> _dataParameter;
        private Mock<IDataParameterCollection> _dataParmeterCollection;

        [SetUp]
        public void SetUp()
        {
            _connectionManager = new Mock<IDatabaseCommandProvider>();
            _dbCommand = new Mock<IDbCommand>();
            _dataParameter = new Mock<IDbDataParameter>();
            _dataParmeterCollection = new Mock<IDataParameterCollection>();
            _databaseCommandBuilder = new DatabaseCommandBuilder(_connectionManager.Object);

            _connectionManager.Setup(x => x.CreateCommandForCurrentConnection()).Returns(_dbCommand.Object);
            _dbCommand.Setup(x => x.CreateParameter()).Returns(_dataParameter.Object);
            _dbCommand.Setup(x => x.Parameters).Returns(_dataParmeterCollection.Object);
        }

        [Test]
        public void ShouldObtainAFreshCommandForTheCurrentConnection()
        {
            //When
            var command = _databaseCommandBuilder.CreateCommandFor(new DataQuery());
    
            //Then
            _connectionManager.Verify(x => x.CreateCommandForCurrentConnection(), Times.Once());
            Assert.That(command, Is.EqualTo(_dbCommand.Object));
        }

        [Test]
        public void ShouldInitaliseCommandProperties()
        {
            //Given
            var dataQuery = new DataQuery().WithStoredProc("Get_Boobies");

            //When
            _databaseCommandBuilder.CreateCommandFor(dataQuery);

            //Then
            _dbCommand.VerifySet(x => x.CommandText = "Get_Boobies");
            _dbCommand.VerifySet(x => x.CommandType = CommandType.StoredProcedure);
        }

        [Test]
        public void ShouldInitialiseCommandDataParameters()
        {
            //Given
            var dataQuery = new DataQuery()
                .WithParam("Title", "Mr")
                .WithParam("FirstName", "David")
                .WithParam("Surname", "Miranda");

            //When
            _databaseCommandBuilder.CreateCommandFor(dataQuery);

            //Then
            _dataParameter.VerifySet(x => x.ParameterName = "Title", Times.Once());
            _dataParameter.VerifySet(x => x.Value =  "Mr", Times.Once());
            _dataParameter.VerifySet(x => x.ParameterName = "FirstName", Times.Once());
            _dataParameter.VerifySet(x => x.Value =  "David", Times.Once());
            _dataParameter.VerifySet(x => x.ParameterName = "Surname", Times.Once());
            _dataParameter.VerifySet(x => x.Value = "Miranda", Times.Once());
        }
    }
}