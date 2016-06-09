using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess.Stubs
{
    public class DataReaderStub : IDatabaseReader
    {
        public IDictionary<string,object> RowData = new Dictionary<string, object>();
        public bool AllowMultipleResults;

        public virtual T Get<T>(string fieldName)
        {
            return ((RowData != null) && (RowData.Count > 0)) ? ((T)RowData[fieldName]) : (default(T));    
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

        public virtual T GetOrDefault<T>(string fieldName)
        {
            if (RowData.ContainsKey(fieldName))
            {
                object fieldObject = Get<T>(fieldName);
                if (fieldObject != DBNull.Value)
                {
                    return (T)fieldObject;
                }
            }
            return default(T);
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

        public bool Closed;
        public void Close()
        {
            Closed = true;
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public virtual bool NextResult()
        {
            return AllowMultipleResults;
        }

        public bool CanRead = true;
        public int ReadCount { get; private set; }

        public virtual bool Read()
        {
            ReadCount++;
            return (this.RowData != null && (this.ReadCount <= this.RowData.Count));
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
