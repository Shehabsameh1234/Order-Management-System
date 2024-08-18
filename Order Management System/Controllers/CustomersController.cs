using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;

namespace Order_Management_System.Controllers
{
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        public CustomersController(ICustomerService customerService,IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Customer>> AddNewCustomer(CustomerDto customer)
        {
            var mappedCustomer= _mapper.Map<CustomerDto,Customer>(customer);

            var result =await _customerService.AddNewCustomer(mappedCustomer);

            if(result ==0) return BadRequest(new ApisResponse(400));

            return Ok(mappedCustomer);
        }
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<CustomerDtoWithOrders>> GetOredersForCustomer(int id)
        {
            var customer =await _customerService.GetCustomer(id);

            if (customer == null) return NotFound(new ApisResponse(404));

            var mappedCustomer = _mapper.Map<Customer, CustomerDtoWithOrders>(customer);

            return Ok(mappedCustomer);
        }
    }
}
