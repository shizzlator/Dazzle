using DataAccess.Interfaces;

namespace DataAccess.Temp
{
    public interface IContactRepository : IRepository
    {
        void Create(Contact contact);
        Contact Get(int contactId);
    }
}