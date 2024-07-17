using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;

namespace Order_Management_System.Controllers
{
  
    public class InvoicesController : BaseApiController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public InvoicesController(IInvoiceService invoiceService,IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(IReadOnlyList<InvoiceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InvoiceDto>>> GetInvoices()
        {
            var Invoices = await _invoiceService.GetAllInvoicesAsync();

            if (Invoices == null) return NotFound(new ApisResponse(404));

            return Ok(_mapper.Map<IReadOnlyList<Invoice>, IReadOnlyList<InvoiceDto>>(Invoices));
        }

        [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var Invoice = await _invoiceService.GetInvoiceAsync(id);

            if (Invoice == null) return NotFound(new ApisResponse(404));

            return Ok(_mapper.Map<Invoice,InvoiceDto>(Invoice));
        }
    }
}
