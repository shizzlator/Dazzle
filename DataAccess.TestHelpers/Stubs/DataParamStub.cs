using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.TestHelpers.Stubs
{
    public class DataParamStub : IDataParam
    {
        public ParameterDirection? Direction { get; set; }

        public int? Size { get; set; }

        public SqlDbType? Type { get; set; }

        public object Value { get; set; }


        public DataParamStub()
        {
        }

        public DataParamStub(object value)
        {
            Value = value;
        }
    }
}
