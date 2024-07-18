using OrderSys.Core.Entities;


namespace OrderSys.Core.Service.Contract
{
    public interface ICustomerService
    {
        Task<int> AddNewCustomer(Customer customer);
        Task<Customer?> GetCustomer(int id);
    }
}
