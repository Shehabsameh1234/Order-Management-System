using OrderSys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Service.Contract
{
    public interface IOrderService
    {
        Task<Order?> CreateNewOrder(Order order);
        Task<Order?> GetOrderData(int id);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatus(Order order);

    }
}
