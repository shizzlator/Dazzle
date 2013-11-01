using System;
using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    internal class TransactionManager : ITransactionManager
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider; 
        private IDbTransaction _transaction;

        public TransactionManager(IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public void Begin()
        {
            if(!TransactionInProgress)
                _transaction = _databaseConnectionProvider.GetOpenConnection().BeginTransaction();
        }

        public IDbTransaction Transaction { get { return _transaction; } }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
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
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public bool TransactionInProgress { get { return _transaction != null; } }
    }
}