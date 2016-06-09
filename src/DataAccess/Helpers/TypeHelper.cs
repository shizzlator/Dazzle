using System;
using System.Data;

namespace DataAccess.Helpers
{
    public static class TypeHelper
    {
        public static SqlDbType ToSqlDbType(this Type t)
        {
            if (t == typeof(DateTime))
            {
                return SqlDbType.DateTime;
            }

            throw new Exception("Parameter type not supported");
        }
    }
}
