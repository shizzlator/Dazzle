using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DataQueryBuilder : IDataQueryBuilder
    {
        private readonly IDictionary<string, object> _parameters = new Dictionary<string, object>();
        private string _commandText;


        public IDataQueryBuilder WithCommandText(string commandText)
        {
            _commandText = commandText;
            _parameters.Clear();
            return this;
        }

        public IDataQueryBuilder WithParam(string name, object value)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }
            _parameters.Add(name, value);
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