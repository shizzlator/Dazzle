using DataAccess.Interfaces;
using DataAccess.Unit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
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
    }
}