using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<ActionResult<Customer>> AddNewCustomer(CustomerDto model)
        {
            var mappedCustomer= _mapper.Map<CustomerDto,Customer>(model);

            var result =await _customerService.AddNewCustomer(mappedCustomer);

            if(result ==0) return BadRequest(new ApisResponse(400));

            return Ok(mappedCustomer);
        }
    }
}
