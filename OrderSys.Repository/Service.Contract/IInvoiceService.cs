using OrderSys.Core.Entities;


namespace OrderSys.Core.Service.Contract
{
    public interface IInvoiceService
    {
        Task<int?> GenerateIvoiceForOrderAsync(Order order);

        Task<IReadOnlyList<Invoice>> GetAllInvoicesAsync();

        Task<Invoice?> GetInvoiceAsync(int id);
    }
}
