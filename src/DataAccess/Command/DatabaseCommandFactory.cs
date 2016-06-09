using System.Data;
using DataAccess.Interfaces;
using DataAccess.Query;

namespace DataAccess.Command
{
    internal class DatabaseCommandFactory : IDatabaseCommandFactory
    {
        private readonly IDatabaseCommandProvider _databaseCommandProvider;

        public DatabaseCommandFactory(IDatabaseCommandProvider databaseCommandProvider)
        {
            _databaseCommandProvider = databaseCommandProvider;
        }

        public IDbCommand CreateCommandFor(IDataQuery dataQuery)
        {
            IDbCommand command = _databaseCommandProvider.CreateCommandForCurrentConnection();
            command.CommandType = dataQuery.CommandType;
            command.CommandText = dataQuery.CommandText;

            if (dataQuery.CommandTimeout.HasValue)
            {
                command.CommandTimeout = (int)dataQuery.CommandTimeout.Value.TotalSeconds;
            }

            if (dataQuery.CommandType == CommandType.StoredProcedure)
                AddParametersForStoredProc(dataQuery, command);
            else
                AddParametersForInlineQuery(dataQuery, command);

            return command;
        }

        private static void AddParametersForInlineQuery(IDataQuery dataQuery, IDbCommand command)
        {
            foreach (IDataParam parameter in dataQuery.Parameters)
            {
                IDbDataParameter dataParameter = command.CreateParameter();

                dataParameter.Value = parameter.Value;
                dataParameter.ParameterName = parameter.Name;
                
                if (parameter.Direction.HasValue)
                {
                    dataParameter.Direction = parameter.Direction.Value;
                }
                if (parameter.Size.HasValue)
                {
                    dataParameter.Size = parameter.Size.Value;
                }
                command.Parameters.Add(dataParameter);
            }
        }

        private static void AddParametersForStoredProc(IDataQuery dataQuery, IDbCommand command)
        {
            foreach (IDataParam parameter in dataQuery.Parameters)
            {
                IDbDataParameter dataParameter = command.CreateParameter();

                dataParameter.ParameterName = parameter.Name;
                dataParameter.Value = parameter.Value;
                MapParameterType(parameter, dataParameter);

                if (parameter.Direction.HasValue)
                {
                    dataParameter.Direction = parameter.Direction.Value;
                }
                if (parameter.Size.HasValue)
                {
                    dataParameter.Size = parameter.Size.Value;
                }
                command.Parameters.Add(dataParameter);
            }
        }

        private static void MapParameterType(IDataParam parameter, IDbDataParameter dataParameter)
        {
            DbType dbtype;
            if (parameter.Type.HasValue)
            {
                dataParameter.DbType = parameter.Type.Value;
            }
            else if (parameter.Value != null && TypeMap.TryGetDbType(parameter.Value.GetType(), out dbtype))
            {
                dataParameter.DbType = dbtype;
            }
        }
    }
}