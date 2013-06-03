using DataAccess.Interfaces;
using NUnit.Framework;
using StructureMap;

namespace DataAccess.TestHelpers
{
    public class TransactionalTestFixture
    {
        protected virtual string ConnectionString { get; set; }
        protected virtual IUnitOfWork Transaction { get; set; }
        protected virtual DataAccessStructureMapWireUp DataAccessStructureMapWireUp { get; set; }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            DataAccessStructureMapWireUp = new DataAccessStructureMapWireUp();
            DataAccessStructureMapWireUp.Initialise(ObjectFactory.Container, ConnectionString);
            Transaction = ObjectFactory.GetInstance<IUnitOfWork>();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Transaction.RollbackAndCloseConnection();
        }
    }
}