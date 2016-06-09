using System;
using System.Data;
using DataAccess.Command;
using DataAccess.Interfaces;
using DataAccess.Query;
using Moq;
using NUnit.Framework;

namespace DataAccess.Tests.Unit
{
    [TestFixture]
    public class DatabaseCommandFactoryTest
    {
        private DatabaseCommandFactory _databaseCommandFactory;
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
            _databaseCommandFactory = new DatabaseCommandFactory(_connectionManager.Object);

            _connectionManager.Setup(x => x.CreateCommandForCurrentConnection()).Returns(_dbCommand.Object);
            _dbCommand.Setup(x => x.CreateParameter()).Returns(_dataParameter.Object);
            _dbCommand.Setup(x => x.Parameters).Returns(_dataParmeterCollection.Object);
        }

        [Test]
        public void ShouldObtainAFreshCommandForTheCurrentConnection()
        {
            //When
            var command = _databaseCommandFactory.CreateCommandFor(new DataQuery());
    
            //Then
            _connectionManager.Verify(x => x.CreateCommandForCurrentConnection(), Times.Once());
            Assert.That(command, Is.EqualTo(_dbCommand.Object));
        }

        [Test]
        public void ShouldInitaliseCommandProperties()
        {
            //Given
            var dataQuery = new DataQuery().WithStoredProc("Get_Shizzle");

            //When
            _databaseCommandFactory.CreateCommandFor(dataQuery);

            //Then
            _dbCommand.VerifySet(x => x.CommandText = "Get_Shizzle");
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
            _databaseCommandFactory.CreateCommandFor(dataQuery);

            //Then
            _dataParameter.VerifySet(x => x.ParameterName = "Title", Times.Once());
            _dataParameter.VerifySet(x => x.Value =  "Mr", Times.Once());
            _dataParameter.VerifySet(x => x.ParameterName = "FirstName", Times.Once());
            _dataParameter.VerifySet(x => x.Value =  "David", Times.Once());
            _dataParameter.VerifySet(x => x.ParameterName = "Surname", Times.Once());
            _dataParameter.VerifySet(x => x.Value = "Miranda", Times.Once());
        }

        [Test]
        public void ShouldNotSetDbTypeForNullValue()
        {
            var dataQuery = new DataQuery()
                .WithParam("Title", null);

            _databaseCommandFactory.CreateCommandFor(dataQuery);

            _dataParameter.VerifySet(x => x.DbType = It.IsAny<DbType>(), Times.Never());
        }

        [Test]
        public void ShouldNotSetDbTypeForDbNullValue()
        {
            var dataQuery = new DataQuery()
                .WithCommandType(CommandType.StoredProcedure)
                .WithParam("Title", DBNull.Value);

            _databaseCommandFactory.CreateCommandFor(dataQuery);

            _dataParameter.VerifySet(x => x.DbType = It.IsAny<DbType>(), Times.Never());
        }

        [Test]
        public void ShouldMapDbTypeForValue()
        {
            var dataQuery = new DataQuery()
                .WithCommandType(CommandType.StoredProcedure)
                .WithParam("Title", "Dr");

            _databaseCommandFactory.CreateCommandFor(dataQuery);

            _dataParameter.VerifySet(x => x.DbType = DbType.String, Times.Once());
        }



        [Test]
        public void ShouldInitaliseCommandTimeout()
        {
            //Given
            var dataQuery = new DataQuery().WithCommandTimeout(TimeSpan.FromHours(1));

            //When
            _databaseCommandFactory.CreateCommandFor(dataQuery);

            //Then
            _dbCommand.VerifySet(x => x.CommandTimeout = 3600);
        }

        [Test]
        public void ShouldInitaliseNotCommandTimeoutWhenNotSet()
        {
            //Given
            var dataQuery = new DataQuery();

            //When
            _databaseCommandFactory.CreateCommandFor(dataQuery);

            //Then
            _dbCommand.VerifySet(x => x.CommandTimeout = It.IsAny<int>(), Times.Never());
        }
    }
}