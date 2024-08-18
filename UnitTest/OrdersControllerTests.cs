using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using FakeItEasy;
using FluentAssertions;
using OrderSys.Service.OrderService;
using Order_Management_System.Errors;


namespace UnitTest
{
    public class OrdersControllerTests
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IInvoiceService _invoiceService;
        private readonly OrdersController _ordersController;
        public OrdersControllerTests()
        {
            _orderService=A.Fake<IOrderService>();
            _mapper=A.Fake<IMapper>();
            _invoiceService=A.Fake<IInvoiceService>();
            _ordersController = new OrdersController(_orderService,_mapper,_invoiceService);
        }
        [Fact]
        public async Task OrdersController_CreateOrder_ReturnOk()
        {
            //Arrange
            var orderDto = new OrderDto();
            var order =new Order();
            A.CallTo(()=>_mapper.Map<OrderDto,Order>(orderDto)).Returns(order);
            A.CallTo(() =>_orderService.CreateNewOrder(order)).Returns(Task.FromResult(order));
            A.CallTo(() => _mapper.Map<Order, OrderDto>(order)).Returns(orderDto);
            //Act
            var result = await _ordersController.CreateOrder(orderDto);
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(orderDto);
        }
        [Fact]
        public async Task OrdersController_CreateOrder_ReturnBadRequest()
        {
            //Arrange
            var orderDto = new OrderDto();
            var order = new Order();
            A.CallTo(() => _mapper.Map<OrderDto, Order>(orderDto)).Returns(order);
            A.CallTo(() => _orderService.CreateNewOrder(order)).Returns(Task.FromResult<Order>(null));
            A.CallTo(() => _mapper.Map<Order, OrderDto>(order)).Returns(orderDto);
            //Act
            var result = await _ordersController.CreateOrder(orderDto);
            //Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
                
        }
        [Fact]
        public async Task OrdersController_GetOrder_ReturnOk()
        {
            //Arrange
            int id = 1;
            var orderDto = new OrderDto();
            var order = new Order();
            A.CallTo(() =>_orderService.GetOrderData(id)).Returns(Task.FromResult(order));
            A.CallTo(() => _mapper.Map<Order, OrderDto>(order)).Returns(orderDto);
            //Act
            var result = await _ordersController.GetOrder(id);
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(orderDto);
        }
        [Fact]
        public async Task OrdersController_GetOrder_ReturnNotFound()
        {
            //Arrange
            int id = 1;
            var orderDto = new OrderDto();
            var order = new Order();
            A.CallTo(() => _orderService.GetOrderData(id)).Returns(Task.FromResult<Order>(null));
            A.CallTo(() => _mapper.Map<Order, OrderDto>(order)).Returns(orderDto);
            //Act
            var result = await _ordersController.GetOrder(id);
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public async Task OrdersController_GetOrders_ReturnOk()
        {
            //Arrange

            IReadOnlyList<OrderDto> orderDto = A.Fake<IReadOnlyList<OrderDto>>();
            IReadOnlyList<Order> order = A.Fake<IReadOnlyList<Order>>();

            A.CallTo(() => _orderService.GetAllOrdersAsync()).Returns(Task.FromResult(order));
            A.CallTo(() => _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderDto>>(order)).Returns(orderDto);
            //Act
            var result = await _ordersController.GetOrders();
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(orderDto);

        }
        [Fact]
        public async Task OrdersController_GetOrders_ReturnNotFound()
        {
            //Arrange

            IReadOnlyList<OrderDto> orderDto = A.Fake<IReadOnlyList<OrderDto>>();
            IReadOnlyList<Order> order = A.Fake<IReadOnlyList<Order>>();

            A.CallTo(() => _orderService.GetAllOrdersAsync()).Returns(Task.FromResult<IReadOnlyList<Order>>(null));
            A.CallTo(() => _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderDto>>(order)).Returns(orderDto);
            //Act
            var result = await _ordersController.GetOrders();
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
    
        }
        [Fact]
        public async Task OrdersController_UpdateOrderSatus_ReturnOk()
        {
            //Arrange
            int id = 1;
            var orderDto = new OrderDto();
            var order = new Order();
            A.CallTo(() =>_orderService.GetOrderData(id)).Returns(Task.FromResult(order));
            A.CallTo(() =>_orderService.UpdateOrderStatus(order)).Returns(Task.FromResult(order));
            A.CallTo(() => _invoiceService.GenerateIvoiceForOrderAsync(order)).Returns(1);
            A.CallTo(() => _mapper.Map<Order, OrderDto>(order)).Returns(orderDto);

            //Act
            var result = await _ordersController.UpdateOrderSatus(id);
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(orderDto);

        }
        [Fact]
        public async Task OrdersController_UpdateOrderSatus_ReturnNotFound_OrderIsNull()
        {
            //Arrange
            int id = 1;
            var orderDto = new OrderDto();
            var order = new Order();
            
            A.CallTo(() => _orderService.GetOrderData(id)).Returns(Task.FromResult<Order>(null));
 
            //Act
            var result = await _ordersController.UpdateOrderSatus(id);
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(404));

        }
        [Fact]
        public async Task OrdersController_UpdateOrderSatus_ReturnBadRequest_OrderIsPlaced()
        {
            //Arrange
            int id = 1;
            var orderDto = new OrderDto();
            var order = new Order() { Status=OrderStatus.placed };
            A.CallTo(() => _orderService.GetOrderData(id)).Returns(Task.FromResult(order));
            //Act
            var result = await _ordersController.UpdateOrderSatus(id);
            //Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(400));

        }
        [Fact]
        public async Task OrdersController_UpdateOrderSatus_ReturnBadRequest_OrderUpdatedFailed()
        {
            //Arrange
            int id = 1;
            var orderDto = new OrderDto();
            var order = new Order();
            A.CallTo(() => _orderService.GetOrderData(id)).Returns(Task.FromResult(order));
            A.CallTo(() => _orderService.UpdateOrderStatus(order)).Returns(Task.FromResult<Order>(null));
            //Act
            var result = await _ordersController.UpdateOrderSatus(id);
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(404));

        }
    }
   
}


