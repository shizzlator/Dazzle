using DataAccess.Interfaces;
using DataAccess.Query;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories;
using Moq;
using NUnit.Framework;

namespace ExampleUsages.UnitTests
{
    [TestFixture]
    public class ContactRepositoryTests
    {
        private Mock<IDatabaseSession> _databaseSession;
        private ContactRepository _contactRepository;
        private Contact _contact;
        private Mock<IDatabaseReader> _dataReader;

        [SetUp]
        public void SetUp()
        {
            _contact = new Contact() { FirstName = "David", Surname = "Miranda", Telephone = "666" };
            _dataReader = new Mock<IDatabaseReader>();
            _dataReader.Setup(x => x.Read()).Returns(true);
            _dataReader.Setup(x => x.Get<string>("FirstName")).Returns("David");
            _dataReader.Setup(x => x.Get<string>("Surname")).Returns("Miranda");
            _dataReader.Setup(x => x.Get<string>("Telephone")).Returns("666");
            
            _databaseSession = new Mock<IDatabaseSession>();
            _databaseSession.Setup(x => x.CreateQuery()).Returns(new DataQuery());
            _contactRepository = new ContactRepository(_databaseSession.Object);
        }

        [Test]
        public void ShouldCreateNewContact()
        {
            _databaseSession.Setup(x => x.ExecuteScalar(It.IsAny<IDataQuery>())).Returns(1024);

            _contact.Id = _contactRepository.Create(_contact);

            Assert.That(_contact.Id, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldGetContact()
        {
            _databaseSession.Setup(x => x.ExecuteReader(It.IsAny<IDataQuery>())).Returns(_dataReader.Object);

            var contact = _contactRepository.Get(5);

            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("666"));
        }
    }
}