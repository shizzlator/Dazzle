using System;
using System.Data;
using DataAccess.Interfaces;
using DataAccess.Query;

namespace DataAccess.Session
{
    public class DatabaseSession : IDatabaseSession
    {
        private readonly IDatabaseCommandFactory _databaseCommandFactory;
        private readonly ITransactionManager _transactionManager;
        private readonly IDatabaseReaderFactory _databaseReaderFactory;
        public IConnectionHandler _connectionHandler;

        public IDbConnection Connection { get { return _connectionHandler.Connection; } }

        internal DatabaseSession(IDatabaseCommandFactory databaseCommandFactory, ITransactionManager transactionManager, IDatabaseReaderFactory databaseReaderFactory, IConnectionHandler connectionHandler)
        {
            _databaseCommandFactory = databaseCommandFactory;
            _transactionManager = transactionManager;
            _databaseReaderFactory = databaseReaderFactory;
            _connectionHandler = connectionHandler;
        }

        public IDataQuery CreateQuery()
        {
            return new DataQuery();
        }

        public object ExecuteScalar(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    using (_connectionHandler.GetHandler(dbCommand, _transactionManager))
                    {
                        return dbCommand.ExecuteScalar();
                    }
                }
            }
            catch(Exception)
            {
                _connectionHandler.CleanUp();
                throw;
            }
        }

        public int ExecuteUpdate(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    using (_connectionHandler.GetHandler(dbCommand, _transactionManager))
                    {
                        return dbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                _connectionHandler.CleanUp();
                throw;
            }
        }

        public IDataParameter ExecuteUpdate(IDataQuery dataQuery, string outputDataParameter)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    using (_connectionHandler.GetHandler(dbCommand, _transactionManager))
                    {
                        dbCommand.ExecuteNonQuery();
                        return (IDataParameter)dbCommand.Parameters[outputDataParameter];    
                    }
                }
            }
            catch (Exception)
            {
                _connectionHandler.CleanUp();
                throw;
            }
        }

        public IDatabaseReader ExecuteReader(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    if (_transactionManager.TransactionInProgress)
                        return _databaseReaderFactory.CreateDataReader(dbCommand.ExecuteReader());
                        
                    return _databaseReaderFactory.CreateDataReader(dbCommand.ExecuteReader(CommandBehavior.CloseConnection));
                }
            }
            catch (Exception)
            {
                _connectionHandler.CleanUp();
                throw;
            }
        }

        public void BeginTransaction()
        {
            _transactionManager.Begin();
        }

        public void CommitTransaction()
        {
            _transactionManager.Commit();
        }

        public void RollbackTransaction()
        {
            _transactionManager.Rollback();
        }
    }
}