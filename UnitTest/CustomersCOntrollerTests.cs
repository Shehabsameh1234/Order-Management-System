using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using OrderSys.Service.CustomerService;


namespace UnitTest
{
    public class CustomersControllerTests
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly CustomersController _customersController;
        public CustomersControllerTests()
        {
            _customerService = A.Fake<ICustomerService>();
            _mapper = A.Fake<IMapper>();
            _customersController = new CustomersController(_customerService,_mapper);

        }
        [Fact]
        public async Task CustomersController_AddNewCustomer_ReturnOk()
        {
            //Arrange
            var customerDto = new CustomerDto();
            var customer = new Customer ();

            A.CallTo(() => _mapper.Map<CustomerDto, Customer>(customerDto)).Returns(customer);
            A.CallTo(() => _customerService.AddNewCustomer(customer)).Returns(Task.FromResult(1)); 

            // Act
            var result = await _customersController.AddNewCustomer(customerDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(customer);
        }
        [Fact]
        public async Task CustomersController_AddNewCustomer_ReturnBadRequest()
        {
            //Arrange
            var customerDto = new CustomerDto();
            var customer = new Customer();

            A.CallTo(() => _mapper.Map<CustomerDto, Customer>(customerDto)).Returns(customer);
            A.CallTo(() => _customerService.AddNewCustomer(customer)).Returns(Task.FromResult(0)); //

            // Act
            var result = await _customersController.AddNewCustomer(customerDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
                
        }
        [Fact]
        public async Task CustomersController_GetOredersForCustomer_ReturnOk()
        {
            //Arrange
            var customerId = 1;
            HashSet<Order> orders = new HashSet<Order>(4)
            { 
                new Order{Id=1},
            };

            var customer = new Customer()
            { 
                Orders=orders
            };

            var customerDto = new CustomerDtoWithOrders();
            A.CallTo(()=>_customerService.GetCustomer(customerId)).Returns(Task.FromResult(customer));
            A.CallTo(()=>_mapper.Map<CustomerDtoWithOrders>(customer)).Returns(customerDto);
            //Act
            var result = await _customersController.GetOredersForCustomer(customerId);
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(customerDto);
        }
        [Fact]
        public async Task CustomersController_GetOredersForCustomer_ReturnNotFound()
        {
            //Arrange
            var customerId = 1;
            var customer = new Customer();
            var customerDto = new CustomerDtoWithOrders();
            A.CallTo(() => _customerService.GetCustomer(customerId)).Returns(Task.FromResult<Customer>(null));
            //Act
            var result = await _customersController.GetOredersForCustomer(customerId);
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
                
        }
        [Fact]
        public async Task CustomersController_GetOredersForCustomer_ReturnNotFound_CustomerHasNoOrders()
        {
            //Arrange
            var customerId = 1;
            HashSet<Order> orders = new HashSet<Order>(0);
            var customer = new Customer()
            {
                Orders = orders
            };
            
            A.CallTo(() => _customerService.GetCustomer(customerId)).Returns(Task.FromResult(customer));
            //Act
            var result = await _customersController.GetOredersForCustomer(customerId);
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();

        }
    }

}
