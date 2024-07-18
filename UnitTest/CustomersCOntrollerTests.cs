using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;


namespace UnitTest
{
    public class CustomersCOntrollerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomersController _controller;

        public CustomersCOntrollerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _mockMapper = new Mock<IMapper>();

            _controller = new CustomersController(
                _mockCustomerService.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task AddNewCustomer_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customerDto = new CustomerDto { /* setup customerDto properties */ };
            var customer = new Customer { /* setup customer properties */ };

            _mockMapper.Setup(m => m.Map<CustomerDto, Customer>(customerDto)).Returns(customer);
            _mockCustomerService.Setup(c => c.AddNewCustomer(customer)).ReturnsAsync(1); // Assuming 1 is a success result

            // Act
            var result = await _controller.AddNewCustomer(customerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(customerDto.Id, model.Id); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task AddNewCustomer_InvalidCustomer_ReturnsBadRequest()
        {
            // Arrange
            var customerDto = new CustomerDto { /* setup invalid customerDto properties */ };

            // Act
            var result = await _controller.AddNewCustomer(customerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrdersForCustomer_ExistingCustomerId_ReturnsOk()
        {
            // Arrange
            int customerId = 1;
            var customer = new Customer { /* setup customer properties */ };

            _mockCustomerService.Setup(c => c.GetCustomer(customerId)).ReturnsAsync(customer);
            _mockMapper.Setup(m => m.Map<Customer, CustomerDtoWithOrders>(customer))
                       .Returns(new CustomerDtoWithOrders { /* setup mapped customerDto properties */ });

            // Act
            var result = await _controller.GetOredersForCustomer(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<CustomerDtoWithOrders>(okResult.Value);
            Assert.Equal(customerId, model.Id); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task GetOrdersForCustomer_NonExistingCustomerId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;

            _mockCustomerService.Setup(c => c.GetCustomer(nonExistingId)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetOredersForCustomer(nonExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
