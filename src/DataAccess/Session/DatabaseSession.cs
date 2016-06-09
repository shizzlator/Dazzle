using System;
using System.Data;
using DataAccess.Interfaces;
using DataAccess.Query;

namespace DataAccess.Session
{
    public class DatabaseSession : IDatabaseSession
    {
        private readonly IDatabaseCommandFactory _databaseCommandFactory;
        private readonly ITransactionWrapper _transactionWrapper;
        private readonly IDatabaseReaderFactory _databaseReaderFactory;
        private readonly IConnectionHandler _connectionHandler;
		private bool _isDisposing;

        internal DatabaseSession(IDatabaseCommandFactory databaseCommandFactory, ITransactionWrapper transactionWrapper, IDatabaseReaderFactory databaseReaderFactory, IConnectionHandler connectionHandler)
        {
            _databaseCommandFactory = databaseCommandFactory;
            _transactionWrapper = transactionWrapper;
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
                    using (_connectionHandler.GetHandler(dbCommand, _transactionWrapper))
                    {
                        return dbCommand.ExecuteScalar();
                    }
                }
            }
            catch(Exception)
            {
                _connectionHandler.RollbackTransactionAndCloseConnection();
                throw;
            }
        }

        public int ExecuteUpdate(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    using (_connectionHandler.GetHandler(dbCommand, _transactionWrapper))
                    {
                        return dbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                _connectionHandler.RollbackTransactionAndCloseConnection();
                throw;
            }
        }

        public IDataParameter ExecuteUpdate(IDataQuery dataQuery, string outputDataParameter)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    using (_connectionHandler.GetHandler(dbCommand, _transactionWrapper))
                    {
                        dbCommand.ExecuteNonQuery();
                        return (IDataParameter)dbCommand.Parameters[outputDataParameter];    
                    }
                }
            }
            catch (Exception)
            {
                _connectionHandler.RollbackTransactionAndCloseConnection();
                throw;
            }
        }

        public IDatabaseReader ExecuteReader(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    if (_transactionWrapper.TransactionInProgress)
                        return _databaseReaderFactory.CreateDataReader(dbCommand.ExecuteReader());
                        
                    return _databaseReaderFactory.CreateDataReader(dbCommand.ExecuteReader(CommandBehavior.CloseConnection));
                }
            }
            catch (Exception)
            {
                _connectionHandler.RollbackTransactionAndCloseConnection();
                throw;
            }
        }

        public void BeginTransaction()
        {
            _transactionWrapper.Begin();
        }

        public void CommitTransaction()
        {
            _transactionWrapper.Commit();
        }

        public void RollbackTransaction()
        {
            _transactionWrapper.Rollback();
        }

		public void Dispose()
		{
			if (!_isDisposing)
			{
				_isDisposing = true;
				_connectionHandler.Dispose();
			}
		}
	}
}