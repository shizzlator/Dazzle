using DataAccess.Interfaces;
using DataAccess.Tests.Unit.Fakes;
using Moq;
using NUnit.Framework;

namespace DataAccess.Tests.Unit
{
    [TestFixture]
    public class RepositoryContainerTest
    {
        [Test]
         public void ShouldCreateInstanceOfDesiredRepository()
        {
            var fakeDatabaseSession = new Mock<IDatabaseSession>();
            var testRepository = new RepositoryFactory().GetInstanceOf<FakeRepository>(fakeDatabaseSession.Object);

             Assert.That(testRepository, Is.Not.Null);
         }

        [Test]
        public void ShouldCreateInstanceOfDesiredRepositoryWithInjectedConntectionString()
        {
            var fakeDatabaseSession = new Mock<IDatabaseSession>();
            var testRepository = new RepositoryFactory("myconnectionstring").GetInstanceOf<FakeRepository>(fakeDatabaseSession.Object);

            Assert.That(testRepository, Is.Not.Null);
        }
    }
}