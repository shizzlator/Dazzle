using System.Data;
using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DatabaseCommandCreatorTest
    {
        private static string _commandText = "command text";
        private static CommandType _commandType = CommandType.Text;
        private Mock<IDatabaseCommandProvider> _connectionManager;
        private DatabaseCommandCreator _databaseCommandCreator;
        private DataQuery _dataQuery;
        private Mock<IDbCommand> _command;
        private Mock<IDbDataParameter> _dataParameter;
        private Mock<IDataParameterCollection> _dataParameterCollection;
        private Mock<ITransactionManager> _transactionManager;

        [SetUp]
        public void SetUp()
        {
            _connectionManager = new Mock<IDatabaseCommandProvider>();
            _transactionManager = new Mock<ITransactionManager>();
            _databaseCommandCreator = new DatabaseCommandCreator(_connectionManager.Object);
            _dataQuery = new DataQuery() {CommandText = _commandText };
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
            //When
            _databaseCommandCreator.CreateCommandFor(_dataQuery);

            //Then
            _connectionManager.Verify(x => x.CreateCommandForCurrentConnection(), Times.Once());
        }

        [Test]
        public void ShouldAddAllParametersToCommand()
        {
            //Given
            _dataQuery.WithParam("@FirstName", "David").WithParam("@Surname", "Miranda");

            //When
            _databaseCommandCreator.CreateCommandFor(_dataQuery);
    
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
            var command = _databaseCommandCreator.CreateCommandFor(_dataQuery);
    
            //Then
            
        }
    }
}