using OrderSys.Core.Entities;


namespace OrderSys.Core.Service.Contract
{
    public interface IOrderService
    {
        Task<Order?> CreateNewOrder(Order order);
        Task<Order?> GetOrderData(int id);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatus(Order order);

        void sendEmailToCustomer(Order order);

    }
}
