using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
