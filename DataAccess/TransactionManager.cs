using System;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class TransactionManager : ITransactionManager
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        [ThreadStatic] 
        private static IDbTransaction _transientTransaction;

        public TransactionManager(IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public void Begin()
        {
            if(!TransactionInProgress)
                _transientTransaction = _databaseConnectionProvider.GetOpenConnection().BeginTransaction();
        }

        public IDbTransaction TransientTransaction { get { return _transientTransaction; } }
        public void Commit()
        {
            try
            {
                _transientTransaction.Commit();
                _transientTransaction.Dispose();
                _transientTransaction = null;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            if (TransactionInProgress)
            {
                _transientTransaction.Rollback();
                _transientTransaction.Dispose();
                _transientTransaction = null;
            }
        }

        public bool TransactionInProgress { get { return _transientTransaction != null; } }

        public void CommitAndDisposeConnection()
        {
            try
            {
                _transientTransaction.Commit();
                _transientTransaction.Dispose();
                _transientTransaction = null;
                _databaseConnectionProvider.GetOpenConnection().Dispose();
            }
            catch (Exception)
            {
                RollbackAndDisposeConnection();
                throw;
            }
        }

        public void RollbackAndDisposeConnection()
        {
            if (TransactionInProgress)
            {
                _transientTransaction.Rollback();
                _transientTransaction.Dispose();
                _transientTransaction = null;
                _databaseConnectionProvider.GetOpenConnection().Dispose();
            }
            else
            {
                _databaseConnectionProvider.GetOpenConnection().Dispose();
            }
        }
    }
}