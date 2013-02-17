using System.Data;

namespace DataAccess
{
    public class DatabaseTransaction
    {
        private IDbConnection _dbProviderConnection;

        public DatabaseTransaction(IDbConnectionFactory dbConnectionFactory)
        {
            _dbProviderConnection = dbConnectionFactory.Create();
        }

        public IDbCommand TransactionForStoredProcedure(IDataQuery dataQuery, IDbTransaction transaction)
        {
            if (transaction == null)
            {
                _dbProviderConnection.Open();
                transaction = _dbProviderConnection.BeginTransaction();
            }
            _command = _dbProviderConnection.CreateCommand();
            _command.CommandType = CommandType.StoredProcedure;
            _command.CommandText = dataQuery.Name;
            AddParametersForStoredProc(dataQuery);
            return _command;
        }
    }
}