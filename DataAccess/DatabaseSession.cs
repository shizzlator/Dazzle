using System;
using System.Data;
using System.Data.SqlClient;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseSession : IDatabaseSession
    {
        private readonly IDatabaseCommandFactory _databaseCommandFactory;
        private readonly ITransactionManager _transactionManager;

        public DatabaseSession(IDatabaseCommandFactory databaseCommandFactory, ITransactionManager transactionManager)
        {
            _databaseCommandFactory = databaseCommandFactory;
            _transactionManager = transactionManager;
        }

        public object RunScalarCommandFor(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    return dbCommand.ExecuteScalar();
                }
            }
            catch(Exception)
            {
                if(_transactionManager.TransactionInProgress)
                    _transactionManager.Rollback();
                //TODO: Log Exception
                throw;
            }
        }

        public int RunUpdateCommandFor(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    return dbCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                if (_transactionManager.TransactionInProgress)
                    _transactionManager.Rollback();
                //TODO: Log Exception
                throw;
            }
        }

        public IDataParameter RunUpdateCommandFor(IDataQuery dataQuery, string outputDataParameter)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    dbCommand.ExecuteNonQuery();
                    return (IDataParameter)dbCommand.Parameters[outputDataParameter];
                }
            }
            catch (Exception)
            {
                if (_transactionManager.TransactionInProgress)
                    _transactionManager.Rollback();
                //TODO: Log Exception
                throw;
            }
        }

        public IDataReader RunReaderFor(IDataQuery dataQuery)
        {
            using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
            {
                return dbCommand.ExecuteReader();
            }
        }
    }
}