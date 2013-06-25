using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.TestHelpers
{
    public class FakeDataQueryBuilder : IDataQueryBuilder
    {
        public IDictionary<string, IDataParam> Parameters { get; private set; }
        public string CommandText { get; set; }

        public FakeDataQueryBuilder()
        {
            Parameters = new Dictionary<string, IDataParam>();
        }

        public IDataQueryBuilder WithCommandText(string commandText)
        {
            CommandText = commandText;
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
            Parameters.Add(name, new DataParam { Value = value, Direction = direction, Size = size, Type = type });
            return this;
        }

        public IDataQuery BuildStoredQuery()
        {
            return new DataQuery();
        }

        public IDataQuery BuildTextQuery()
        {
            return new DataQuery();
        }
    }
}