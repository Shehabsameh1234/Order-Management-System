using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;


namespace UnitTest
{
    public class InvoicesControllersTests
    {
        private readonly Mock<IInvoiceService> _mockInvoiceService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly InvoicesController _controller;

        public InvoicesControllersTests()
        {
            _mockInvoiceService = new Mock<IInvoiceService>();
            _mockMapper = new Mock<IMapper>();

            _controller = new InvoicesController(
                _mockInvoiceService.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetInvoices_ReturnsOk()
        {
            // Arrange
            var invoices = new List<Invoice> { /* setup list of invoices */ };

            _mockInvoiceService.Setup(s => s.GetAllInvoicesAsync()).ReturnsAsync(invoices);
            _mockMapper.Setup(m => m.Map<IReadOnlyList<Invoice>, IReadOnlyList<InvoiceDto>>(invoices))
                       .Returns(new List<InvoiceDto> { /* setup mapped invoiceDto list */ });

            // Act
            var result = await _controller.GetInvoices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<List<InvoiceDto>>(okResult.Value);
            Assert.Equal(invoices.Count, model.Count); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task GetInvoices_NoInvoices_ReturnsNotFound()
        {
            // Arrange
            _mockInvoiceService.Setup(s => s.GetAllInvoicesAsync()).ReturnsAsync((List<Invoice>)null);

            // Act
            var result = await _controller.GetInvoices();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetInvoice_ExistingInvoiceId_ReturnsOk()
        {
            // Arrange
            int invoiceId = 1;
            var invoice = new Invoice { /* setup invoice properties */ };

            _mockInvoiceService.Setup(s => s.GetInvoiceAsync(invoiceId)).ReturnsAsync(invoice);
            _mockMapper.Setup(m => m.Map<Invoice, InvoiceDto>(invoice)).Returns(new InvoiceDto { /* setup mapped invoiceDto properties */ });

            // Act
            var result = await _controller.GetInvoice(invoiceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<InvoiceDto>(okResult.Value);
            Assert.Equal(invoiceId, model.Id); // Example assertion, adjust as per your scenario
        }

        [Fact]
        public async Task GetInvoice_NonExistingInvoiceId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;

            _mockInvoiceService.Setup(s => s.GetInvoiceAsync(nonExistingId)).ReturnsAsync((Invoice)null);

            // Act
            var result = await _controller.GetInvoice(nonExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
