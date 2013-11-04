using System.Configuration;
using DataAccess.Query;
using DataAccess.TestHelpers;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories;
using NUnit.Framework;

namespace DataAccess.Integration.Tests
{
    //TO RUN this locally, execute the run the Sql Script in ExampleUsages/DBScript!!!
    [TestFixture]
    public class ContactRepositoryTest : TransactionalTestFixture
    {
        private int _contactId;

        private Contact _contact;

        protected override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["ConnectionString"]; }
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _contact = new Contact {FirstName = "David", Surname = "Miranda", Telephone = "999"};
            _contactId = UnitOfWork.Repository<ContactRepository>().Create(_contact);
        }

        [Test]
        public void ShouldGetContactUsingRunQueryMethodWithInlineParameterisedSql()
        {
            //When - This could be a generic repository
            var contact = UnitOfWork.Repository<ContactRepository>().RunQuery("select * from Contact where FirstName = @FirstName", new QueryParameters("@FirstName", "David"));

            //Then
            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        [Test]
        public void ShouldGetContactUsingRunQueryMethodWithDataQuery()
        {
            //Given
            var dataQuery = new DataQuery() {CommandText = "select * from Contact where FirstName = @FirstName",}.WithParam("@FirstName", "David");

            //When - This could be a generic repository
            var contact = UnitOfWork.Repository<ContactRepository>().RunQuery(dataQuery);

            //Then
            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        [Test]
        public void ShouldGetContactUsingCustomGetMehthod()
        {
            //When
            var contact = UnitOfWork.Repository<ContactRepository>().Get(_contactId);

            //Then
            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        [Test, Ignore] //Needs stored proc
        public void ShouldCreateContactUsingOutputParameter()
        {
            //Uses output parameter
            var contact = UnitOfWork.Repository<ContactRepository>().CreateContact(_contact);

            UnitOfWork.Repository<ContactRepository>().Get(contact.Id);

            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }
    }
}
