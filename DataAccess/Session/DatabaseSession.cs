using System;
using System.Data;
using DataAccess.Interfaces;
using DataAccess.Query;

namespace DataAccess.Session
{
    public class DatabaseSession : IDatabaseSession
    {
        private readonly IDatabaseCommandBuilder _databaseCommandBuilder;
        private readonly ITransactionManager _transactionManager;

        public DatabaseSession(IDatabaseCommandBuilder databaseCommandBuilder, ITransactionManager transactionManager)
        {
            _databaseCommandBuilder = databaseCommandBuilder;
            _transactionManager = transactionManager;
        }

        public IDataQuery CreateQuery()
        {
            return new DataQuery();
        }

        public object ExecuteScalar(IDataQuery dataQuery)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandBuilder.CreateCommandFor(dataQuery))
                {
                    connection = dbCommand.Connection;
                    var result = dbCommand.ExecuteScalar();

                    if(!_transactionManager.TransactionInProgress)
                        connection.Close();

                    return result;
                }
            }
            catch(Exception)
            {
                CleanUp(connection);
                throw;
            }
        }

        public int ExecuteUpdate(IDataQuery dataQuery)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandBuilder.CreateCommandFor(dataQuery))
                {
                    connection = dbCommand.Connection;
                    var result = dbCommand.ExecuteNonQuery();

                    if (!_transactionManager.TransactionInProgress)
                        connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {
                CleanUp(connection);
                throw;
            }
        }

        public IDataParameter ExecuteUpdate(IDataQuery dataQuery, string outputDataParameter)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandBuilder.CreateCommandFor(dataQuery))
                {
                    connection = dbCommand.Connection;
                    dbCommand.ExecuteNonQuery();
                    var result = (IDataParameter) dbCommand.Parameters[outputDataParameter];

                    if (!_transactionManager.TransactionInProgress)
                        connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {
                CleanUp(connection);
                throw;
            }
        }

        public IDataReader ExecuteReader(IDataQuery dataQuery)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandBuilder.CreateCommandFor(dataQuery))
                {
                    connection = dbCommand.Connection;

                    if (_transactionManager.TransactionInProgress)
                        return dbCommand.ExecuteReader();
                    else
                        return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception)
            {
                CleanUp(connection);
                throw;
            }
        }

        private void CleanUp(IDbConnection connection)
        {
            if (_transactionManager.TransactionInProgress)
                _transactionManager.Rollback();
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public void BeginTransaction()
        {
            if (!_transactionManager.TransactionInProgress)
            {
                _transactionManager.Begin();
            }
        }

        public void CommitTransaction()
        {
            _transactionManager.Commit();
        }

        public void CommitAndCloseConnection()
        {
            _transactionManager.Commit();
        }

        public void RollbackTransaction()
        {
            _transactionManager.Rollback();
        }
    }
}