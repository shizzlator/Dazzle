using DataAccess;
using DataAccess.Interfaces;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories;

namespace ExampleUsages
{
    public class RepositoryFactoryUsage
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public RepositoryFactoryUsage(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public RepositoryFactoryUsage() : this(new RepositoryFactory("connectionString!"))
        {   
        }

        public void SaveContact(Contact contact)
        {
            _repositoryFactory.GetInstanceOf<ContactRepository>().Save(contact);
        }
    }
}