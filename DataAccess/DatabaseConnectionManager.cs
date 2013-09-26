﻿using System.Data;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseConnectionManager : IDatabaseConnectionManager
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;
        private readonly ITransactionManager _transactionManager;
        private IDbConnection _dbConnection;

        public DatabaseConnectionManager(IDatabaseConnectionProvider databaseConnectionProvider, ITransactionManager transactionManager)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
            _transactionManager = transactionManager;
        }

        public IDbCommand CreateCommandForCurrentConnection()
        {
            _dbConnection = _databaseConnectionProvider.GetOpenConnection();
            return GetCommand();
        }

        private IDbCommand GetCommand()
        {
            var command = _dbConnection.CreateCommand();
            if (_transactionManager.TransactionInProgress)
                command.Transaction = _transactionManager.TransientTransaction;
            return command;
        }
    }
}