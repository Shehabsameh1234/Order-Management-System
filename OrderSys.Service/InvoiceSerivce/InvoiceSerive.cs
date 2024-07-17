using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;

namespace OrderSys.Service.InvoiceSerivce
{
    public class InvoiceSerive : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceSerive(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int?> GenerateIvoiceForOrderAsync(Order order)
        {
            var Invoice = new Invoice()
            { 
                OrderId =order.Id,
                TotalAmount = order.TotalAmount,
            };

            _unitOfWork.Repository<Invoice>().Add(Invoice);

            var result = await _unitOfWork.CompleteAsync();

            if (result == 0) return null;

            return result;
        }

        public async Task<IReadOnlyList<Invoice>> GetAllInvoicesAsync()
        {
            var invoices = await  _unitOfWork.Repository<Invoice>().GetAllAsync();

            return invoices;
        }

        public Task<Invoice?> GetInvoiceAsync(int id)
        {
            var invoice = _unitOfWork.Repository<Invoice>().GetAsync(id);

            return invoice;
        }
    }
}
