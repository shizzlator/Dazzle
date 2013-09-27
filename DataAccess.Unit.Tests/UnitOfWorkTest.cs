using DataAccess.Interfaces;
using DataAccess.Unit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class UnitOfWorkTest
    {
        private Mock<IRepositoryContainer> _repositoryContainer;
        private FakeRepository _expectedRepository;
        private UnitOfWork _unitOfWork;
        private Mock<ITransactionManager> _transactionManager;
        private Mock<IDatabaseSessionFactory> _databaseSessionFactory;

        [SetUp]
        public void SetUp()
        {
            _repositoryContainer = new Mock<IRepositoryContainer>();
            _databaseSessionFactory = new Mock<IDatabaseSessionFactory>();
            _expectedRepository = new FakeRepository(_databaseSessionFactory.Object);
            _transactionManager = new Mock<ITransactionManager>();
            _unitOfWork = new UnitOfWork(_databaseSessionFactory.Object, new Mock<IRepositoryContainer>().Object);
        }

        [Test]
        public void ShouldGetARepositoryFromTheIoCContainer()
        {
            //Given
            _repositoryContainer.Setup(x => x.GetInstanceOf<FakeRepository>()).Returns(_expectedRepository);

            //When
            var testRepository = _unitOfWork.Repository<FakeRepository>();

            //Then
            _repositoryContainer.Verify(x => x.GetInstanceOf<FakeRepository>(), Times.Once());
            Assert.That(testRepository, Is.EqualTo(_expectedRepository));
        }

        [Test]
        public void ShouldCreateNewTransaction()
        {
            _repositoryContainer.Setup(x => x.GetInstanceOf<FakeRepository>()).Returns(_expectedRepository);

            //When
            _unitOfWork.Repository<FakeRepository>();

            //Then
            _transactionManager.Verify(x => x.Begin(), Times.Once());
        }

        [Test]
        public void ShouldNotBeginATransactionIfOneIsInProgress()
        {
            //Given
            _repositoryContainer.Setup(x => x.GetInstanceOf<FakeRepository>()).Returns(_expectedRepository);
            _transactionManager.SetupGet(x => x.TransactionInProgress).Returns(true);

            //When
            _unitOfWork.Repository<FakeRepository>();

            //Then
            _transactionManager.Verify(x => x.Begin(), Times.Never());
        }
    }
}