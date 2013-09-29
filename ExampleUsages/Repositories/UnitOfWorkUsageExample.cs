using DataAccess;
using DataAccess.Interfaces;
using ExampleUsages.DTOs;

namespace ExampleUsages.Repositories
{
    public class UnitOfWorkUsageExample
    {
        private IUnitOfWork _unitOfWork;

        public UnitOfWorkUsageExample(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void SaveContact(Contact contact)
        {
            _unitOfWork.Repository<ContactRepository>().Save(contact);
        }

        public void UnitOfWorkWireUp()
        {
            //this can be done in your IoC or poor mans for each repository
            _unitOfWork = new UnitOfWork("Connection String!!!");
        }
    }
}