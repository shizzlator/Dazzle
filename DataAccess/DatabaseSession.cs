using System;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseSession : IDatabaseSession
    {
        private readonly IDatabaseCommandCreator _databaseCommandCreator;
        private readonly ITransactionManager _transactionManager;

        public DatabaseSession(IDatabaseCommandCreator databaseCommandCreator, ITransactionManager transactionManager)
        {
            _databaseCommandCreator = databaseCommandCreator;
            _transactionManager = transactionManager;
        }

        public IDataQuery CreateQuery()
        {
            return new DataQuery();
        }

        public object RunScalarCommandFor(IDataQuery dataQuery)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandCreator.CreateCommandFor(dataQuery))
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
                //TODO: Log Exception
                throw;
            }
        }

        public int RunUpdateCommandFor(IDataQuery dataQuery)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandCreator.CreateCommandFor(dataQuery))
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
                //TODO: Log Exception
                throw;
            }
        }

        public IDataParameter RunUpdateCommandFor(IDataQuery dataQuery, string outputDataParameter)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandCreator.CreateCommandFor(dataQuery))
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
                //TODO: Log Exception
                throw;
            }
        }

        public IDataReader RunReaderFor(IDataQuery dataQuery)
        {
            IDbConnection connection = null;
            try
            {
                using (var dbCommand = _databaseCommandCreator.CreateCommandFor(dataQuery))
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
                //TODO: Log Exception
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