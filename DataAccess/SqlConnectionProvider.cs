using System;
using System.Data;
using System.Data.SqlClient;
using DataAccess.Interfaces;

namespace DataAccess
{
    internal class SqlConnectionProvider : IDatabaseConnectionProvider
    {
        [ThreadStatic]
        private static IDbConnection _connection;
        private readonly string _connectionString;

        public SqlConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            InitialiseConnection();
            OpenConnection();
            return _connection;
        }

        private static void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void InitialiseConnection()
        {
            if (_connection == null || string.IsNullOrEmpty(_connection.ConnectionString))
            {
                _connection = new SqlConnection(_connectionString);
            }
        }

        public static IDbConnection Connection
        {
            get { return _connection; }
        }
    }
}