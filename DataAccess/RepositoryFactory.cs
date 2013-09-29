using System;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public T GetInstanceOf<T>(IDatabaseSession databaseSession) where T : IRepository
        {
            return (T)Activator.CreateInstance(typeof(T), databaseSession);
        }
    }
}