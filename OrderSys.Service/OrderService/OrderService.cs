using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
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

        //public async Task<int> CreateNewOrder(Order order)
        //{




        //    _unitOfWork.Repository<Order>().Add(order);

        //    var result =await _unitOfWork.CompleteAsync();

        //    return result;
        //}
    }
}
