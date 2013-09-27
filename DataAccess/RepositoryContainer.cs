using System;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class RepositoryContainer : IRepositoryContainer
    {
        private readonly IDatabaseSessionFactory _databaseSessionFactory;

        public RepositoryContainer(IDatabaseSessionFactory databaseSessionFactory)
        {
            _databaseSessionFactory = databaseSessionFactory;
        }

        public T GetInstanceOf<T>()
        {
            return (T)Activator.CreateInstance(typeof(T), _databaseSessionFactory.CreateSession());
        }
    }
}