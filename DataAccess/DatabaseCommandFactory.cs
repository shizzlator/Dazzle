using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseCommandFactory : IDatabaseCommandFactory
    {
        private readonly IDatabaseConnectionManager _connectionManager;
        private IDbCommand _dbCommand;

        public DatabaseCommandFactory(IDatabaseConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public IDbCommand CreateCommandFor(IDataQuery dataQuery)
        {
            InitialiseCommand(dataQuery);
            AddParametersForStoredProc(dataQuery);
            return _dbCommand;
        }

        private void InitialiseCommand(IDataQuery dataQuery)
        {
            _dbCommand = _connectionManager.CreateCommandForCurrentConnection();
            _dbCommand.CommandType = dataQuery.CommandType;
            _dbCommand.CommandText = dataQuery.CommandText;
        }

        private void AddParametersForStoredProc(IDataQuery dataQuery)
        {
            foreach (var parameter in dataQuery.Parameters)
            {
                IDbDataParameter dataParameter = _dbCommand.CreateParameter();
                dataParameter.ParameterName = parameter.Key;
                dataParameter.Value = parameter.Value;
                _dbCommand.Parameters.Add(dataParameter);
            }
        }
    }
}