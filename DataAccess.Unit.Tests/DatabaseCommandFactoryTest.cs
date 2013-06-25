using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseCommandFactoryTest
    {
        private static string _commandText = "command text";
        private static CommandType _commandType = CommandType.Text;
        private Mock<IDatabaseConnectionManager> _connectionManager;
        private DatabaseCommandFactory _databaseCommandFactory;
        private DataQuery _dataQuery;
        private Mock<IDbCommand> _command;
        private Mock<IDbDataParameter> _dataParameter;
        private Mock<IDataParameterCollection> _dataParameterCollection;
        private Mock<ITransactionManager> _transactionManager;

        [SetUp]
        public void SetUp()
        {
            _connectionManager = new Mock<IDatabaseConnectionManager>();
            _transactionManager = new Mock<ITransactionManager>();
            _databaseCommandFactory = new DatabaseCommandFactory(_connectionManager.Object);
            _dataQuery = new DataQuery() {CommandText = _commandText, CommandType = _commandType};
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
            //Given
            _command.SetupProperty(x => x.CommandText);
            _command.SetupProperty(x => x.CommandType);

            //When
            var command = _databaseCommandFactory.CreateCommandFor(_dataQuery);

            //Then
            Assert.That(command.CommandText, Is.EqualTo(_commandText));
            Assert.That(command.CommandType, Is.EqualTo(_commandType));
            _connectionManager.Verify(x => x.CreateCommandForCurrentConnection(), Times.Once());
        }

        [Test]
        public void ShouldAddAllParametersToCommand()
        {
            //Given
            _dataQuery.AddParam("@FirstName", "David").AddParam("@Surname", "Miranda");

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

        [Test]
        public void ShouldUseTransientTransactionToCreateCommand()
        {
            //Given
            var dbTransaction = new Mock<IDbTransaction>().Object;
            _transactionManager.SetupGet(x => x.TransactionInProgress).Returns(true);

            //When
            var command = _databaseCommandFactory.CreateCommandFor(_dataQuery);
    
            //Then
            
        }
    }
}