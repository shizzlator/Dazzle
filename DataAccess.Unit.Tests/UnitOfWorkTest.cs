using DataAccess.Interfaces;
using DataAccess.Unit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class UnitOfWorkTest
    {
        private Mock<IRepositoryFactory> _repositoryFactory;
        private Mock<IDatabaseSessionFactory> _databaseSessionFactory;
        private Mock<IDatabaseSession> _databaseSession;
        private FakeRepository _expectedRepository;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            _databaseSession = new Mock<IDatabaseSession>();
            _repositoryFactory = new Mock<IRepositoryFactory>();
            _databaseSessionFactory = new Mock<IDatabaseSessionFactory>();
            _expectedRepository = new FakeRepository(_databaseSession.Object);

            _databaseSessionFactory.Setup(x => x.CreateSession()).Returns(_databaseSession.Object);

            _unitOfWork = new UnitOfWork(_databaseSessionFactory.Object, _repositoryFactory.Object);
        }

        [Test]
        public void ShouldGetARepository()
        {
            //Given
            _repositoryFactory.Setup(x => x.GetInstanceOf<FakeRepository>(_databaseSession.Object)).Returns(_expectedRepository);

            //When
            var testRepository = _unitOfWork.Repository<FakeRepository>();

            //Then
            Assert.That(testRepository, Is.EqualTo(_expectedRepository));
        }

        [Test]
        public void ShouldWrapCallInTransaction()
        {
            //Given
            _repositoryFactory.Setup(x => x.GetInstanceOf<FakeRepository>(_databaseSession.Object)).Returns(_expectedRepository);

            //When
            _unitOfWork.Repository<FakeRepository>();

            //Then
            _databaseSession.Verify(x => x.BeginTransaction(), Times.Once());
        }
    }
}