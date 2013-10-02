using System.Data;
using DataAccess.Command;
using DataAccess.Interfaces;
using DataAccess.Query;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseCommandFactoryTest
    {
        private Mock<IDatabaseCommandInstaceProvider> _connectionManager;
        private DatabaseCommandFactory _databaseCommandFactory;
        private DataQuery _dataQuery;
        private Mock<IDbCommand> _command;
        private Mock<IDbDataParameter> _dataParameter;
        private Mock<IDataParameterCollection> _dataParameterCollection;
        private Mock<ITransactionManager> _transactionManager;

        [SetUp]
        public void SetUp()
        {
            _connectionManager = new Mock<IDatabaseCommandInstaceProvider>();
            _transactionManager = new Mock<ITransactionManager>();
            _databaseCommandFactory = new DatabaseCommandFactory(_connectionManager.Object);
            _dataQuery = new DataQuery();
            _command = new Mock<IDbCommand>();
            _dataParameter = new Mock<IDbDataParameter>();
            _dataParameterCollection = new Mock<IDataParameterCollection>();
            _command.Setup(x => x.Parameters).Returns(_dataParameterCollection.Object);
            _command.Setup(x => x.CreateParameter()).Returns(_dataParameter.Object);
            _connectionManager.Setup(x => x.CreateCommandForCurrentConnection()).Returns(_command.Object);
            
        }

        [Test]
        public void ShouldInitialiseCommand()
        {
            _dataQuery.WithStoredProc("GetShizzle");

            //When
            _databaseCommandFactory.CreateCommandFor(_dataQuery);

            //Then
            _connectionManager.Verify(x => x.CreateCommandForCurrentConnection(), Times.Once());
            _command.VerifySet(x => x.CommandText = "GetShizzle");
            _command.VerifySet(x => x.CommandType = CommandType.StoredProcedure);
        }

        [Test]
        public void ShouldAddAllParametersToCommand()
        {
            //Given
            _dataQuery.WithParam("@FirstName", "David").WithParam("@Surname", "Miranda");

            //When
            _databaseCommandFactory.CreateCommandFor(_dataQuery);
    
            //Then
            _command.Verify(x => x.CreateParameter(), Times.Exactly(2));
            _dataParameter.VerifySet(x => x.ParameterName = "@FirstName");
            _dataParameter.VerifySet(x => x.Value = "David");
            _dataParameter.VerifySet(x => x.ParameterName = "@Surname");
            _dataParameter.VerifySet(x => x.Value = "Miranda");
            _dataParameterCollection.Verify(x => x.Add(_dataParameter.Object), Times.Exactly(2));
        }
    }
}