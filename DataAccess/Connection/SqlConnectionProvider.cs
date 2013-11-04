using System.Data;
using System.Data.SqlClient;
using DataAccess.Interfaces;

namespace DataAccess.Connection
{
    internal class SqlConnectionProvider : IDatabaseConnectionProvider
    {
		private IDbConnection _connection;
        private readonly string _connectionString;
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public SqlConnectionProvider(string connectionString) : this(connectionString, new DatabaseConnectionFactory())
        {
            _connectionString = connectionString;
        }

        internal SqlConnectionProvider(string connectionString, IDatabaseConnectionFactory connectionFactory)
        {
            _connectionString = connectionString;
            _connectionFactory = connectionFactory;
        }

        public IDbConnection GetOpenConnection()
        {
            InitialiseConnection();
            OpenConnection();
            return _connection;
        }

        private void OpenConnection()
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
                _connection = _connectionFactory.SqlConnection(_connectionString);
            }
        }
    }
}