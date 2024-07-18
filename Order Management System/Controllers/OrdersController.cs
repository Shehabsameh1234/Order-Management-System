using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Service.Contract;


namespace Order_Management_System.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IInvoiceService _invoiceService;
        public OrdersController(IOrderService orderService,IMapper mapper,IInvoiceService invoiceService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _invoiceService = invoiceService;
        }
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            //set the paymentMethod and status
            orderDto.Status = OrderStatus.Pending.ToString()    ;
            if (orderDto.PaymentMethod != PaymentMethods.Paypal.ToString() && orderDto.PaymentMethod != PaymentMethods.CreditCard.ToString())
                orderDto.PaymentMethod = PaymentMethods.Cash.ToString();

            var mappedOrder = _mapper.Map<OrderDto, Order>(orderDto);

            var result = await _orderService.CreateNewOrder(mappedOrder);

            if (result == null) return BadRequest(new ApisResponse(400));

            return Ok(_mapper.Map<Order, OrderDto>(mappedOrder));
        }
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderData(id);

            if (order == null) return NotFound(new ApisResponse(404));

            return Ok(_mapper.Map<Order, OrderDto>(order));
        }
        [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            if (orders == null) return NotFound(new ApisResponse(404));

            return Ok(_mapper.Map < IReadOnlyList<Order>, IReadOnlyList<OrderDto>>(orders));
        }
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult<OrderDto>> UpdateOrderSatus(int id)
        {
            var order = await _orderService.GetOrderData(id);

            if (order == null) return NotFound(new ApisResponse(404));

            if(order.Status == OrderStatus.placed) return BadRequest(new ApisResponse(400));

            order.Status = OrderStatus.placed;

            var updatedOrder =await _orderService.UpdateOrderStatus(order);

            if(updatedOrder == null) return NotFound(new ApisResponse(404));

            await _invoiceService.GenerateIvoiceForOrderAsync(updatedOrder);

            _orderService.sendEmailToCustomer(updatedOrder);

            return Ok(_mapper.Map<Order , OrderDto>(updatedOrder));
        }
    }
}
