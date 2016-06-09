using DataAccess.Interfaces;

namespace DataAccess.Tests.Unit.Fakes
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