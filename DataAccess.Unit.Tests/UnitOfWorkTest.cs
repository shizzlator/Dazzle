using DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class UnitOfWorkTest
    {
        private Mock<IObjectContainer> _iocContainer;
        private TestRepository _expectedRepository;
        private UnitOfWork _unitOfWork;
        private Mock<ITransactionManager> _transactionManager;

        [SetUp]
        public void SetUp()
        {
            _iocContainer = new Mock<IObjectContainer>();
            _expectedRepository = new TestRepository();
            _transactionManager = new Mock<ITransactionManager>();
            _unitOfWork = new UnitOfWork(_iocContainer.Object, _transactionManager.Object);
        }

        [Test]
        public void ShouldGetARepositoryFromTheIoCContainer()
        {
            //Given
            _iocContainer.Setup(x => x.GetInstanceOf<TestRepository>()).Returns(_expectedRepository);

            //When
            var testRepository = _unitOfWork.Repository<TestRepository>();

            //Then
            _iocContainer.Verify(x => x.GetInstanceOf<TestRepository>(), Times.Once());
            Assert.That(testRepository, Is.EqualTo(_expectedRepository));
        }

        [Test]
        public void ShouldCreateNewTransaction()
        {
            _iocContainer.Setup(x => x.GetInstanceOf<TestRepository>()).Returns(_expectedRepository);

            //When
            _unitOfWork.Repository<TestRepository>();

            //Then
            _transactionManager.Verify(x => x.Begin(), Times.Once());
        }

        [Test]
        public void ShouldNotBeginATransactionIfOneIsInProgress()
        {
            //Given
            _iocContainer.Setup(x => x.GetInstanceOf<TestRepository>()).Returns(_expectedRepository);
            _transactionManager.SetupGet(x => x.TransactionInProgress).Returns(true);

            //When
            _unitOfWork.Repository<TestRepository>();

            //Then
            _transactionManager.Verify(x => x.Begin(), Times.Never());
        }
    }

    public class TestRepository : IRepository
    {
    }
}