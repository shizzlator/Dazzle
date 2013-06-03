using System.Configuration;
using DataAccess.TestHelpers;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories;
using ExampleUsages.Repositories.Interfaces;
using NUnit.Framework;
using StructureMap;

namespace DataAccess.Integration.Tests
{
    public class ContactRepositoryTest : TransactionalTestFixture
    {
        private int _contactId;
        private Contact _contact;

        public ContactRepositoryTest()
        {
            ObjectFactory.Container.Configure(x => x.For<IContactRepository>().Use<ContactRepository>());
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _contact = new Contact {FirstName = "David", Surname = "Miranda", Telephone = "999"};
            _contactId = Transaction.Repository<ContactRepository>().Create(_contact);
        }

        [Test]
        public void ShouldGetContactUsingRunQueryMethodWithInlineParameterisedSQL()
        {
            //When - This could be a generic repository
            var contact = Transaction.Repository<ContactRepository>().RunQuery("select * from Contact where FirstName = @FirstName", new QueryParameters("@FirstName", "David"));

            //Then
            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        [Test]
        public void ShouldGetContactUsingRunQueryMethodWithDataQuery()
        {
            //Given
            var dataQuery = new DataQuery() {CommandText = "select * from Contact where FirstName = @FirstName",}.AddParam("@FirstName", "David");

            //When - This could be a generic repository
            var contact = Transaction.Repository<ContactRepository>().RunQuery(dataQuery);

            //Then
            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        [Test]
        public void ShouldGetContactUsingCustomGetMehthod()
        {
            //When
            var contact = Transaction.Repository<ContactRepository>().Get(_contactId);

            //Then
            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        [Test, Ignore] //Needs stored proc
        public void ShouldCreateContactUsingOutputParameter()
        {
            //Uses output parameter
            var contact = Transaction.Repository<ContactRepository>().CreateContact(_contact);

            Transaction.Repository<ContactRepository>().Get(contact.Id);

            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        protected override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["ConnectionString"]; }
        }
    }
}
