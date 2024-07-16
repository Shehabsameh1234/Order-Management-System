using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;

namespace Order_Management_System.Controllers
{
   
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        //[HttpPost]
        //public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        //{
        //    var mappedOrder =_mapper.Map<OrderDto,Order>(orderDto);

        //    var result=await _orderService.CreateNewOrder(mappedOrder);

        //    if (result != 0) return BadRequest(new ApisResponse(400));

        //    return Ok(mappedOrder);
        //}
    }
}
