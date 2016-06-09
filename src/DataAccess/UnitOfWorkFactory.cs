using DataAccess.Interfaces;

namespace DataAccess
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;

        public UnitOfWorkFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_connectionString);
        }
    }
}
