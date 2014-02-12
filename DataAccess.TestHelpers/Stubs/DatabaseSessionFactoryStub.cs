using DataAccess.Interfaces;

namespace DataAccess.TestHelpers.Stubs
{
    public class DatabaseSessionFactoryStub : IDatabaseSessionFactory
    {
        public IDatabaseSession SessionValue { get; set; }

        public IDatabaseSession CreateSession()
        {
            return SessionValue ?? new DatabaseSessionStub();
        }

        public IDatabaseSession CreateSession(string connectionString)
        {
            return SessionValue ?? new DatabaseSessionStub();
        }
    }
}
