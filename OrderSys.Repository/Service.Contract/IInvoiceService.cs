using OrderSys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Service.Contract
{
    public interface IInvoiceService
    {
        Task<int?> GenerateIvoiceForOrderAsync(Order order);

        Task<IReadOnlyList<Invoice>> GetAllInvoicesAsync();

        Task<Invoice?> GetInvoiceAsync(int id);
    }
}
