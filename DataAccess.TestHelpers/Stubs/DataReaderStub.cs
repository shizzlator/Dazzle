using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.TestHelpers.Stubs
{
    public class DataReaderStub : IDatabaseReader
    {
        public IDictionary<string,object> RowData = new Dictionary<string, object>(); 

        public T Get<T>(string fieldName)
        {
            return (RowData != null && RowData.Count > 0) ? ((T)RowData[fieldName]) : (default(T));    
        }

        public bool GetBool(string fieldName)
        {
            bool result = false;

            if (RowData != null && RowData.Count > 0)
            {
                result = (bool) RowData[fieldName];
            }

            return result;
        }

        public T GetOrDefault<T>(string fieldName)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldOffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        int readCount = 0;
        public bool Read()
        {
            readCount++;
            return (readCount <= 1);
        }

        public int Depth { get; private set; }
        public bool IsClosed { get; private set; }
        public bool IsDisposed { get; set; }
        public int RecordsAffected { get; private set; }
        public int FieldCount { get; private set; }
        
        void IDisposable.Dispose()
        {
            IsDisposed = true;
        }
    }
}
