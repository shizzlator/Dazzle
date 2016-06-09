using System;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    internal class SqlDatabaseReader : IDatabaseReader
    {
        private readonly IDataReader _dataReader;

        public SqlDatabaseReader(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }
        
        public T Get<T>(string fieldName)
        {
            return _dataReader.FieldCount != 0 ? GetField<T>(fieldName) : default(T);
        }

        public bool GetBool(string fieldName)
        {
            return _dataReader.GetBoolean(_dataReader.GetOrdinal(fieldName));
        }

        public T GetOrDefault<T>(string fieldName)
        {
            return _dataReader.FieldCount != 0 ? GetField<T>(fieldName, true) : default(T);
        }

        private T GetField<T>(string fieldName, bool returnDefaultWhenDbNull = false)
        {
            var field = _dataReader[fieldName];
            if (field != null && !IsDBNull(GetOrdinal(fieldName)))
            {
                if (returnDefaultWhenDbNull)
                {
                    if (IsDBNull(GetOrdinal(fieldName)))
                    {
                        return default(T);
                    }
                }

                return (T)field;
            }
            return default(T);
        }

        public void Dispose()
        {
            _dataReader.Dispose();
        }

        public string GetName(int i)
        {
            return _dataReader.GetName(i);
        }

        public string GetDataTypeName(int i)
        {
            return _dataReader.GetDataTypeName(i);
        }

        public Type GetFieldType(int i)
        {
            return _dataReader.GetFieldType(i);
        }

        public object GetValue(int i)
        {
            return _dataReader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return _dataReader.GetValues(values);
        }

        public int GetOrdinal(string name)
        {
            return _dataReader.GetOrdinal(name);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return _dataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public long GetChars(int i, long fieldOffset, char[] buffer, int bufferoffset, int length)
        {
            return _dataReader.GetChars(i, fieldOffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return _dataReader.GetData(i);
        }

        public bool IsDBNull(int i)
        {
            return _dataReader.IsDBNull(i);
        }

        public void Close()
        {
            _dataReader.Close();
        }

        public DataTable GetSchemaTable()
        {
            return _dataReader.GetSchemaTable();
        }

        public bool NextResult()
        {
            return _dataReader.NextResult();
        }

        public bool Read()
        {
            return _dataReader.Read();
        }

        public int Depth { get { return _dataReader.Depth; } }
        public bool IsClosed { get { return _dataReader.IsClosed; } }
        public int RecordsAffected { get { return _dataReader.RecordsAffected; } }
        public int FieldCount { get { return _dataReader.FieldCount; } }
    }
}