using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;



namespace UnitTest
{
    public class InvoicesControllersTests
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        private readonly InvoicesController _invoiceController;
        public InvoicesControllersTests()
        {
            _invoiceService=A.Fake<IInvoiceService>();
            _mapper=A.Fake<IMapper>();
            _invoiceController = new InvoicesController(_invoiceService, _mapper);
        }
        [Fact]
        public async Task InvoicesController_GetInvoices_ReturnOk()
        {
            //Arrange
            var invoices =A.Fake<IReadOnlyList<Invoice>>();
            var invoicesDto = A.Fake<IReadOnlyList<InvoiceDto>>();
            A.CallTo(()=>_invoiceService.GetAllInvoicesAsync()).Returns(Task.FromResult(invoices));
            A.CallTo(() => _mapper.Map<IReadOnlyList<InvoiceDto>>(invoices)).Returns(invoicesDto);
            //Act
            var result = await _invoiceController.GetInvoices();
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

        }
        [Fact]
        public async Task InvoicesController_GetInvoices_ReturnNotFound()
        {
            //Arrange
            A.CallTo(() => _invoiceService.GetAllInvoicesAsync()).Returns(Task.FromResult<IReadOnlyList<Invoice>>(null));
            // Act
            var result = await _invoiceController.GetInvoices();
            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(404));
        }
        [Fact]
        public async Task InvoicesController_GetInvoice_ReturnOk()
        {
            //Arrange
            var invoice = A.Fake<Invoice>();
            var invoiceDto = A.Fake<InvoiceDto>();
            A.CallTo(()=> _invoiceService.GetInvoiceAsync(1)).Returns(Task.FromResult(invoice));
            A.CallTo(() => _mapper.Map<InvoiceDto>(invoice)).Returns(invoiceDto);
            //Act
            var result = await _invoiceController.GetInvoice(1);
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(invoiceDto);
        }
        [Fact]
        public async Task InvoicesController_GetInvoice_ReturnNotFound()
        {
            //Arrange
            A.CallTo(() => _invoiceService.GetInvoiceAsync(1)).Returns(Task.FromResult<Invoice>(null));
            //Act
            var result = await _invoiceController.GetInvoice(1);
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(404));
        }

    }
}
