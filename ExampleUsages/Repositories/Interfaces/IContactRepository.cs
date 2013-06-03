using DataAccess.Interfaces;
using ExampleUsages.DTOs;

namespace ExampleUsages.Repositories.Interfaces
{
    public interface IContactRepository
    {
        int Create(Contact contact);
        Contact Get(int contactId);
    }
}