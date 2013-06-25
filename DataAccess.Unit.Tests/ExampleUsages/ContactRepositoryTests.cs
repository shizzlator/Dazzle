using System.Data;
using DataAccess.Interfaces;
using DataAccess.TestHelpers;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests.ExampleUsages
{
    [TestFixture]
    public class ContactRepositoryTests
    {
        private Mock<IDatabaseSession> _databaseSession;
        private ContactRepository _contactRepository;
        private FakeDataQueryBuilder _dataQueryBuilder;
        private Contact _contact;
        private Mock<IDataReader> _dataReader;

        [SetUp]
        public void SetUp()
        {
            _contact = new Contact() { FirstName = "David", Surname = "Miranda", Telephone = "666" };
            _dataReader = new Mock<IDataReader>();
            _dataReader.Setup(x => x.Read()).Returns(true);
            _dataReader.Setup(x => x["FirstName"]).Returns("David");
            _dataReader.Setup(x => x["Surname"]).Returns("Miranda");
            _dataReader.Setup(x => x["Telephone"]).Returns("666");

            _databaseSession = new Mock<IDatabaseSession>();
            _dataQueryBuilder = new FakeDataQueryBuilder();
            
            _contactRepository = new ContactRepository(_databaseSession.Object, _dataQueryBuilder);
        }

        [Test]
        public void ShouldCreateNewContact()
        {
            _databaseSession.Setup(x => x.RunScalarCommandFor(It.IsAny<IDataQuery>())).Returns(1024);

            _contact.Id = _contactRepository.Create(_contact);

            Assert.That((object) _contact.Id, Is.EqualTo(1024));
        }

        [Test]
        public void ShouldGetContact()
        {
            _databaseSession.Setup(x => x.RunReaderFor(It.IsAny<IDataQuery>())).Returns(_dataReader.Object);

            var contact = _contactRepository.Get(5);

            Assert.That((object) contact.FirstName, Is.EqualTo("David"));
            Assert.That((object) contact.Surname, Is.EqualTo("Miranda"));
            Assert.That((object) contact.Telephone, Is.EqualTo("666"));
        }
    }
}