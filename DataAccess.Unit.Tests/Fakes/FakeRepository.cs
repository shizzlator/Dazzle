using DataAccess.Interfaces;

namespace DataAccess.Unit.Tests.Fakes
{
    public class FakeRepository : IRepository
    {
        private readonly IDatabaseSessionFactory _databaseSessionFactory;

        public FakeRepository(IDatabaseSessionFactory databaseSessionFactory)
        {
            _databaseSessionFactory = databaseSessionFactory;
        }
    }
}