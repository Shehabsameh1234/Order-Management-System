using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IInvoiceService> _mockInvoiceService;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockMapper = new Mock<IMapper>();
            _mockInvoiceService = new Mock<IInvoiceService>();

            _controller = new OrdersController(
                _mockOrderService.Object,
                _mockMapper.Object,
                _mockInvoiceService.Object
            );
        }

        [Fact]
        public async Task CreateOrder_ValidOrder_ReturnsOk()
        {
            // Arrange
            var orderDto = new OrderDto { /* setup orderDto properties */ };
            var order = new Order { /* setup order properties */ };

            _mockMapper.Setup(m => m.Map<OrderDto, Order>(orderDto)).Returns(order);
            _mockOrderService.Setup(o => o.CreateNewOrder(order)).ReturnsAsync(order);

            // Act
            var result = await _controller.CreateOrder(orderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(orderDto.Id, model.Id); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task CreateOrder_InvalidOrder_ReturnsBadRequest()
        {
            // Arrange
            var orderDto = new OrderDto { /* setup invalid orderDto properties */ };

            // Act
            var result = await _controller.CreateOrder(orderDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrder_ExistingOrderId_ReturnsOk()
        {
            // Arrange
            int orderId = 1;
            var order = new Order { /* setup order properties */ };

            _mockOrderService.Setup(o => o.GetOrderData(orderId)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(order.Id, model.Id); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task GetOrder_NonExistingOrderId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;

            _mockOrderService.Setup(o => o.GetOrderData(nonExistingId)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetOrder(nonExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrders_AdminUser_ReturnsOk()
        {
            // Arrange
            var orders = new List<Order> { /* setup list of orders */ };

            _mockOrderService.Setup(o => o.GetAllOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Equal(orders.Count, model.Count); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task GetOrders_NonAdminUser_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.GetOrders();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task UpdateOrderStatus_ExistingOrderId_ReturnsOk()
        {
            // Arrange
            int orderId = 1;
            var order = new Order { /* setup order properties */ };

            _mockOrderService.Setup(o => o.GetOrderData(orderId)).ReturnsAsync(order);
            _mockOrderService.Setup(o => o.UpdateOrderStatus(order)).ReturnsAsync(order);

            // Act
            var result = await _controller.UpdateOrderSatus(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(OrderStatus.placed.ToString(), model.Status); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task UpdateOrderStatus_NonExistingOrderId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;

            _mockOrderService.Setup(o => o.GetOrderData(nonExistingId)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.UpdateOrderSatus(nonExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateOrderStatus_OrderAlreadyPlaced_ReturnsBadRequest()
        {
            // Arrange
            int orderId = 1;
            var order = new Order { /* setup order properties with Status = OrderStatus.Placed */ };

            _mockOrderService.Setup(o => o.GetOrderData(orderId)).ReturnsAsync(order);

            // Act
            var result = await _controller.UpdateOrderSatus(orderId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
