using DataAccess;
using DataAccess.Interfaces;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories;

namespace ExampleUsages
{
    public class UnitOfWorkUsageExample
    {
        private IUnitOfWork _unitOfWork;

        public UnitOfWorkUsageExample(IUnitOfWork unitOfWork)
        {
            //this can be done in your IoC or poor mans
            _unitOfWork = new UnitOfWork("Connection String!!!");
        }

        public void SaveContact(Contact contact)
        {
            _unitOfWork.Repository<ContactRepository>().Save(contact);
        }

    }
}