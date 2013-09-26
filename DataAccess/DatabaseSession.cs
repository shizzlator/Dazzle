using System;
using System.Data;
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

        public IDataQuery CreateQuery()
        {
            return new DataQuery();
        }

        public object RunScalarCommandFor(IDataQuery dataQuery)
        {
            try
            {
                using (var dbCommand = _databaseCommandFactory.CreateCommandFor(dataQuery))
                {
                    var result = dbCommand.ExecuteScalar();
                    if(!_transactionManager.TransactionInProgress)
                        dbCommand.Connection.Close();
                    return result;
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
                    var result = dbCommand.ExecuteNonQuery();
                    if (!_transactionManager.TransactionInProgress)
                        dbCommand.Connection.Close();
                    return result;
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
                    var result = (IDataParameter) dbCommand.Parameters[outputDataParameter];
                    dbCommand.Connection.Close();
                    return result;
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
                if (_transactionManager.TransactionInProgress)
                    return dbCommand.ExecuteReader();
                else
                    return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
}