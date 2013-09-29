using System;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDatabaseReader : IDisposable
    {
        T Get<T>(string fieldName);
        void Dispose();
        string GetName(int i);
        string GetDataTypeName(int i);
        Type GetFieldType(int i);
        object GetValue(int i);
        int GetValues(object[] values);
        int GetOrdinal(string name);
        long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);
        long GetChars(int i, long fieldOffset, char[] buffer, int bufferoffset, int length);
        IDataReader GetData(int i);
        bool IsDBNull(int i);
        void Close();
        DataTable GetSchemaTable();
        bool NextResult();
        bool Read();
        int Depth { get; }
        bool IsClosed { get; }
        int RecordsAffected { get; }
    }
}