using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using OrderSys.Core.Specifications.CustomerSpecifications;
using Talabat.Core;

namespace OrderSys.Service.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddNewCustomer(Customer customer)
        {
            _unitOfWork.Repository<Customer>().Add(customer);

           var result =await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task<Customer?> GetCustomer(int id)
        {
            var spec = new CustomerSpecifications(id);
            var customer =await _unitOfWork.Repository<Customer>().GetWithSpecAsync(spec);
            return customer ;
        }
    }
}
