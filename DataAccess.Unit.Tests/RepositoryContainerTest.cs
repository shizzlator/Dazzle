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
            var databaseSessionFactory = new Mock<IDatabaseSessionFactory>();
            databaseSessionFactory.Setup(x => x.CreateSession()).Returns(new Mock<IDatabaseSession>().Object);

            var testRepository = new RepositoryContainer(databaseSessionFactory.Object);

             Assert.That(testRepository, Is.Not.Null);
         }
    }
}