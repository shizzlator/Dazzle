using System.Data;

namespace DataAccess
{
    public static class ReaderExtensions
    {
        public static string GetString(this IDataReader dataReader, string fieldName)
        {
            return (string)dataReader[fieldName];
        }

        public static int GetInt(this IDataReader dataReader, string fieldName)
        {
            return (int)dataReader[fieldName];
        }

        public static bool GetBool(this IDataReader dataReader, string fieldName)
        {
            return (bool) dataReader[fieldName];
        }

        public static T Get<T>(this IDataReader dataReader, string fieldName)
        {
            return dataReader.FieldCount != 0 ? GetField<T>(dataReader, fieldName) : default(T);
        }

        private static T GetField<T>(this IDataReader dataReader, string fieldName)
        {
            var field = dataReader[fieldName];
            if (field != null)
            {
                return (T) field;
            }
            return default(T);
        }
    }
}