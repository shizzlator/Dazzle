using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DataQueryBuilder : IDataQueryBuilder
    {
        private readonly IDictionary<string, IDataParam> _parameters = new Dictionary<string, IDataParam>();
        private string _commandText;

        public IDataQueryBuilder WithCommandText(string commandText)
        {
            _commandText = commandText;
            _parameters.Clear();
            return this;
        }

        public IDataQueryBuilder WithParam(string name, IDataParam dataParam)
        {
            return WithParam(name, dataParam.Value, dataParam.Direction, dataParam.Size, dataParam.Type);
        }

        public IDataQueryBuilder WithParam(string name, object value)
        {
            return WithParam(name, value, null, null, null);
        }

        public IDataQueryBuilder WithParam(string name, object value, ParameterDirection? direction, int? size, SqlDbType? type)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }
            _parameters.Add(name, new DataParam { Value = value, Direction = direction, Size = size, Type = type});
            return this;
        }

        public IDataQuery BuildStoredQuery()
        {
            return new DataQuery { Parameters = _parameters, CommandType = CommandType.StoredProcedure, CommandText = _commandText };
        }

        public IDataQuery BuildTextQuery()
        {
            return new DataQuery {Parameters = _parameters, CommandType = CommandType.Text, CommandText = _commandText};
        }
    }
}