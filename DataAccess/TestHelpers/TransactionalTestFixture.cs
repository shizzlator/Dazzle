using DataAccess.Interfaces;
using NUnit.Framework;
using StructureMap;

namespace DataAccess.TestHelpers
{
    public class TransactionalTestFixture
    {
        protected virtual string ConnectionString { get; set; }
        protected virtual IUnitOfWork UnitOfWork { get; set; }
        protected virtual DataAccessStructureMapWireUp DataAccessStructureMapWireUp { get; set; }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            DataAccessStructureMapWireUp = new DataAccessStructureMapWireUp();
            DataAccessStructureMapWireUp.Initialise(ObjectFactory.Container, ConnectionString);
            UnitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            UnitOfWork.Rollback();
            UnitOfWork.Dispose();
        }
    }
}