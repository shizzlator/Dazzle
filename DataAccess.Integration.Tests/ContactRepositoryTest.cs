using DataAccess.Temp;
using DataAccess.TestHelpers;
using NUnit.Framework;
using StructureMap;

namespace DataAccess.Integration.Tests
{
    public class ContactRepositoryTest : TransactionalTestFixture
    {
        public ContactRepositoryTest()
        {
            ObjectFactory.Container.Configure(x => x.For<IContactRepository>().Use<ContactRepository>());
        }

        [Test]
        public void ShouldGetContact()
        {
            //Given
            UnitOfWork.Repository<ContactRepository>()
                      .Create(new Contact() {FirstName = "David", Surname = "Miranda", Telephone = "999"});

            //When
            var contact = UnitOfWork.Repository<ContactRepository>()
                .RunQuery("select * from Contact where FirstName = @FirstName", new QueryParameters("@FirstName", "David"));

            //Then
            Assert.That(contact.FirstName, Is.EqualTo("David"));
            Assert.That(contact.Surname, Is.EqualTo("Miranda"));
            Assert.That(contact.Telephone, Is.EqualTo("999"));
        }

        protected override string ConnectionString
        {
            get { return @"Data Source=SHIZZLEBOX;Initial Catalog=ContactList;User Id=sa;Password=bl4ntyr3;"; }
        }
    }
}
