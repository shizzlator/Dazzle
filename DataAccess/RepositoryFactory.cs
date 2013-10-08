using System;
using DataAccess.Interfaces;
using DataAccess.Session;

namespace DataAccess
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string _connectionString;

        public RepositoryFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public RepositoryFactory()
        {    
        }

        public T GetInstanceOf<T>() where T : IRepository
        {
            if(string.IsNullOrEmpty(_connectionString))
                throw new Exception("Connection string has not been intialised");

            return GetInstanceOf<T>(new DatabaseSessionFactory().CreateSession(_connectionString));
        }

        public T GetInstanceOf<T>(string connectionString) where T : IRepository
        {
            return GetInstanceOf<T>(new DatabaseSessionFactory().CreateSession(connectionString));
        }

        public T GetInstanceOf<T>(IDatabaseSession databaseSession) where T : IRepository
        {
            return (T)Activator.CreateInstance(typeof(T), databaseSession);
        }
    }
}