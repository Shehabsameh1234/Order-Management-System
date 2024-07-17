using OrderSys.Core.Entities;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Service.Contract;
using OrderSys.Core.Specifications.OrderSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;

namespace OrderSys.Service.OrderService
{
    public class OrderService :IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> CreateNewOrder(Order order)
        {
            //check customer if exist
            var customer = await _unitOfWork.Repository<Customer>().GetAsync(order.CustomerId);
            if (customer == null) return null; 
            
            //set the right data based on product repo
            foreach (var item in order.OrderItems)
            {
                var product =await _unitOfWork.Repository<Product>().GetAsync(item.ProductId);
                if (product == null) return null;
                item.UnitPrice = product.Price;

                //chaeck the quantity
                if (item.Quantity > product.Stock || item.Quantity == 0) return null;

                //set orderItems value
                order.TotalAmount = order.OrderItems.Sum(o => o.UnitPrice * o.Quantity);
            };

            //set dicsount property based on total amount
            foreach (var item in order.OrderItems)
            {
                if (order.TotalAmount > 100 && order.TotalAmount <= 200) item.Discount = 5M;
                else if (order.TotalAmount > 200) item.Discount = 10M;
                else item.Discount = 0;
            }

            //set the discount on totalAmount
            if (order.TotalAmount > 100 && order.TotalAmount <= 200)
                order.TotalAmount = order.TotalAmount - (order.TotalAmount * 5 / 100);

            else if (order.TotalAmount > 200)
                order.TotalAmount =order.TotalAmount-( order.TotalAmount * 10 / 100);

            //add to dataBase
            _unitOfWork.Repository<Order>().Add(order);

            var result = await _unitOfWork.CompleteAsync();
            if (result == 0) return null;

            return order;
        }

        public Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            var spec = new OrderSpecifications();

            var orders = _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return orders;
        }

        public async Task<Order?> GetOrderData(int id)
        {
            var spec = new OrderSpecifications(id);

            var order =await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            return order;
        }

        public async Task<Order?> UpdateOrderStatus(Order order)
        {
            _unitOfWork.Repository<Order>().Update(order);

            var result = await _unitOfWork.CompleteAsync();

            if (result == 0) return null;

            return order;
        }


    }
}
