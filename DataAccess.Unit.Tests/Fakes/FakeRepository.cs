using DataAccess.Interfaces;

namespace DataAccess.Unit.Tests.Fakes
{
    public class FakeRepository : IRepository
    {
        private readonly IDatabaseSession _databaseSession;

        public FakeRepository(IDatabaseSession databaseSession)
        {
            _databaseSession = databaseSession;
        }
    }
}