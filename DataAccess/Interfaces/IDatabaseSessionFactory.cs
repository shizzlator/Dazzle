using System;

namespace DataAccess.Interfaces
{
    public interface IDatabaseSessionFactory
    {
        IDatabaseSession CreateSession();
    }
}
