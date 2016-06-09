using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Stubs
{
    public class DataParamStub : IDataParam
    {
        public DataParamStub()
        {
        }

        public DataParamStub(object value)
        {
            Value = value;
        }

        public DataParamStub(object value, DbType type)
        {
            Value = value;
            Type = type;
        }

        public ParameterDirection? Direction { get; set; }
        public int? Size { get; set; }
        DbType? IDataParam.Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        DbType? Type { get; set; }
    }
}
