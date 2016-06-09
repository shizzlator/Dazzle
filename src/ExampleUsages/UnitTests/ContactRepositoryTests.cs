using System.Data;
using System.Data.SqlClient;
using DataAccess.Interfaces;
using DataAccess.Query;
using DataAccess.Stubs;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories;
using Moq;
using NUnit.Framework;

namespace ExampleUsages.UnitTests
{
    [TestFixture]
    public class ContactRepositoryTests
    {
        private Mock<IDatabaseSessionFactory> _databaseSessionFactory;
        private ContactRepository _contactRepository;
        private Contact _contact;
        private Mock<IDatabaseReader> _dataReader;
        private Mock<IDatabaseSession> _databaseSession;

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

            _databaseSessionFactory = new Mock<IDatabaseSessionFactory>();
            _databaseSessionFactory.Setup(x => x.CreateSession(It.IsAny<string>())).Returns(_databaseSession.Object);
            
            _contactRepository = new ContactRepository(_databaseSessionFactory.Object);
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

        [Test]
        public void ShouldGetNameAndDirectionOfParameter()
        {
            //Given
            var databaseSessionStub = new DatabaseSessionStub {UpdateResultParameter = new SqlParameter {Value = 12}};
            var databaseSessionFactoryStub = new DatabaseSessionFactoryStub {SessionValue = databaseSessionStub};

            //When
            var contact = new Contact
            {
                FirstName = "David",
                Surname = "Miranda",
                Telephone = "666"
            };
            var contactId = new ContactRepository(databaseSessionFactoryStub).UpdateContact(contact);
            var dbQueries = databaseSessionStub.Queries;

            //Then
            Assert.That(dbQueries[0].Parameters[0].Direction, Is.EqualTo(ParameterDirection.Output));
            Assert.That(dbQueries[0].Parameters[0].Name, Is.EqualTo("@ContactId"));
            Assert.That(dbQueries[0].StoredProcName, Is.EqualTo("proc_name"));
            Assert.That(dbQueries[0].Parameters[1].Value, Is.EqualTo(contact.FirstName));
            Assert.That(dbQueries[0].Parameters[2].Value, Is.EqualTo(contact.Surname));
            Assert.That(dbQueries[0].Parameters[3].Value, Is.EqualTo(contact.Telephone));
            Assert.That(contactId, Is.EqualTo(12));
        }
    }
}