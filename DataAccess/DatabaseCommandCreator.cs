using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseCommandCreator : IDatabaseCommandCreator
    {
        private readonly IDatabaseCommandProvider _databaseCommandProvider;
        private IDbCommand _dbCommand;

        public DatabaseCommandCreator(IDatabaseCommandProvider databaseCommandProvider)
        {
            _databaseCommandProvider = databaseCommandProvider;
        }

        public IDbCommand CreateCommandFor(IDataQuery dataQuery)
        {
            _dbCommand = _databaseCommandProvider.CreateCommandForCurrentConnection();
            _dbCommand.CommandType = dataQuery.CommandType;
            _dbCommand.CommandText = dataQuery.CommandText;

            AddParametersForStoredProc(dataQuery);

            return _dbCommand;
        }

        private void AddParametersForStoredProc(IDataQuery dataQuery)
        {
            foreach (var parameter in dataQuery.Parameters)
            {
                IDbDataParameter dataParameter = _dbCommand.CreateParameter();
                dataParameter.ParameterName = parameter.Key;
                dataParameter.Value = parameter.Value.Value; //TODO: sort this nonsense out!
                if (parameter.Value.Direction.HasValue)
                {
                    dataParameter.Direction = parameter.Value.Direction.Value;
                }
                if (parameter.Value.Size.HasValue)
                {
                    dataParameter.Size = parameter.Value.Size.Value;
                }
                if (parameter.Value.Type.HasValue)
                {
                    dataParameter.DbType = ConvertSqlDbTypeToDbType(parameter.Value.Type.Value);
                }
                _dbCommand.Parameters.Add(dataParameter);
            }
        }

        private DbType ConvertSqlDbTypeToDbType(SqlDbType sqlDbType)
        {
            if (sqlDbType == SqlDbType.Int)
            {
                return DbType.Int32;
            }
            return (DbType) sqlDbType;
        }
    }
}