using DataAccess.Interfaces;
using NUnit.Framework;

namespace DataAccess.TestHelpers
{
    public abstract class TransactionalTestFixture
    {
        abstract protected string ConnectionString { get; }
        protected virtual IUnitOfWork UnitOfWork { get; set; }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            UnitOfWork = new UnitOfWork(ConnectionString);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            UnitOfWork.RollbackAndCloseConnection();
        }
    }
}