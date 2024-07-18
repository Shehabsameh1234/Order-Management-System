using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;


namespace UnitTest
{
    public class ProductsControllerTest
    {
        public class ProductsControllerTests
        {
            private readonly Mock<IProductService> _mockProductService;
            private readonly Mock<IMapper> _mockMapper;
            private readonly ProductsController _controller;

            public ProductsControllerTests()
            {
                _mockProductService = new Mock<IProductService>();
                _mockMapper = new Mock<IMapper>();
                _controller = new ProductsController(_mockProductService.Object, _mockMapper.Object);
            }

            [Fact]
            public async Task GetProducts_ReturnsOkResult()
            {
                // Arrange
                var products = new List<Product> { new Product { Id = 1, Name = "Product 1", Price = 10.99m }, new Product { Id = 2, Name = "Product 2", Price = 15.99m } }; // Sample products list
                _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);
                _mockMapper.Setup(mapper => mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products))
                           .Returns(products.Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price }).ToList());

                // Act
                var result = await _controller.GetProducts();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsAssignableFrom<IReadOnlyList<ProductDto>>(okResult.Value);
                Assert.Equal(products.Count, returnValue.Count);
            }

            [Fact]
            public async Task GetProducts_ReturnsNotFound()
            {
                // Arrange
                List<Product> products = null; // Simulate empty products list
                _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

                // Act
                var result = await _controller.GetProducts();

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                var errorResponse = Assert.IsType<ApisResponse>(notFoundResult.Value);
                Assert.Equal(404, errorResponse.StatusCode);
            }

            [Fact]
            public async Task GetProduct_WithValidId_ReturnsOkResult()
            {
                // Arrange
                int productId = 1;
                var product = new Product { Id = productId, Name = "Test Product", Price = 10.99m }; // Create a test product
                var productDto = new ProductDto { Id = productId, Name = "Test Product", Price = 10.99m }; // Expected DTO

                _mockProductService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync(product);
                _mockMapper.Setup(mapper => mapper.Map<Product, ProductDto>(product)).Returns(productDto);

                // Act
                var result = await _controller.GetProduct(productId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<ProductDto>(okResult.Value);
                Assert.Equal(productId, returnValue.Id);
                Assert.Equal(product.Name, returnValue.Name);
                Assert.Equal(product.Price, returnValue.Price);
            }

            [Fact]
            public async Task GetProduct_WithInvalidId_ReturnsNotFound()
            {
                // Arrange
                int productId = 999; // Invalid product ID
                Product nullProduct = null;
                _mockProductService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync(nullProduct);

                // Act
                var result = await _controller.GetProduct(productId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                var errorResponse = Assert.IsType<ApisResponse>(notFoundResult.Value);
                Assert.Equal(404, errorResponse.StatusCode);
            }

            [Fact]
            public async Task AddNewProduct_ValidProduct_ReturnsOkResult()
            {
                // Arrange
                var productDto = new ProductDto { Name = "New Product", Price = 19.99m }; // Create a test product DTO
                var product = new Product { Id = 1, Name = "New Product", Price = 19.99m }; // Expected product after mapping

                _mockMapper.Setup(mapper => mapper.Map<ProductDto, Product>(productDto)).Returns(product);
                _mockProductService.Setup(service => service.AddProductAsync(product)).ReturnsAsync(1); // Simulate successful addition

                // Act
                var result = await _controller.AddNewProduct(productDto);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<Product>(okResult.Value);
                Assert.Equal(productDto.Name, returnValue.Name);
                Assert.Equal(productDto.Price, returnValue.Price);
            }

            [Fact]
            public async Task AddNewProduct_InvalidProduct_ReturnsBadRequest()
            {
                // Arrange
                var productDto = new ProductDto { Name = "Invalid Product", Price = -5.99m }; // Invalid product DTO (negative price)
                var product = new Product { Name = "Invalid Product", Price = -5.99m }; // Invalid product after mapping

                _mockMapper.Setup(mapper => mapper.Map<ProductDto, Product>(productDto)).Returns(product);
                _mockProductService.Setup(service => service.AddProductAsync(product)).ReturnsAsync(0); // Simulate failure

                // Act
                var result = await _controller.AddNewProduct(productDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorResponse = Assert.IsType<ApisResponse>(badRequestResult.Value);
                Assert.Equal(400, errorResponse.StatusCode);
            }

            [Fact]
            public async Task UpdateProduct_ValidProduct_ReturnsOkResult()
            {
                // Arrange
                var productDto = new ProductDto { Id = 1, Name = "Updated Product", Price = 29.99m }; // Updated product DTO
                var updatedProduct = new Product { Id = 1, Name = "Updated Product", Price = 29.99m }; // Expected updated product after mapping

                _mockMapper.Setup(mapper => mapper.Map<ProductDto, Product>(productDto)).Returns(updatedProduct);
                _mockProductService.Setup(service => service.UpdateProductAsync(updatedProduct)).ReturnsAsync(updatedProduct); // Simulate successful update

                // Act
                var result = await _controller.UpdateProduct(productDto);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<Product>(okResult.Value);
                Assert.Equal(productDto.Id, returnValue.Id);
                Assert.Equal(productDto.Name, returnValue.Name);
                Assert.Equal(productDto.Price, returnValue.Price);
            }

            [Fact]
            public async Task UpdateProduct_InvalidProduct_ReturnsBadRequest()
            {
                // Arrange
                var productDto = new ProductDto { Id = 999, Name = "Invalid Product", Price = -5.99m }; // Invalid product DTO (negative price)
                var invalidProduct = new Product { Id = 999, Name = "Invalid Product", Price = -5.99m }; // Invalid product after mapping

                _mockMapper.Setup(mapper => mapper.Map<ProductDto, Product>(productDto)).Returns(invalidProduct);
                _mockProductService.Setup(service => service.UpdateProductAsync(invalidProduct)).ReturnsAsync((Product)null); // Simulate failure

                // Act
                var result = await _controller.UpdateProduct(productDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorResponse = Assert.IsType<ApisResponse>(badRequestResult.Value);
                Assert.Equal(400, errorResponse.StatusCode);
            }

        }
    }
}