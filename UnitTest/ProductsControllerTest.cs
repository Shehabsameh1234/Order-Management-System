using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using OrderSys.Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class ProductsControllerTest
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ProductsController _productsController;
        public ProductsControllerTest()
        {
            _productService=A.Fake<IProductService>();
            _mapper=A.Fake<IMapper>();
            _productsController = new ProductsController( _productService, _mapper );   

        }
        [Fact]
        public async Task ProductsController_GetProducts_ReturnOk()
        {
            //Arrange
            var products = A.Fake<IReadOnlyList<Product>>();
            var productsDto = A.Fake<IReadOnlyList<ProductDto>>();
            A.CallTo(() => _productService.GetAllProductsAsync()).Returns(products);
            A.CallTo(() => _mapper.Map<IReadOnlyList<ProductDto>>(products)).Returns(productsDto);
            //Act
            var result = await _productsController.GetProducts();
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task ProductsController_GetProducts_ReturnNotFound()
        {
            //Arrange
            List<Product> products = null;
            A.CallTo(() => _productService.GetAllProductsAsync()).Returns(products);
            //Act
            var result = await _productsController.GetProducts();
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();  
        }
        [Fact]
        public async Task ProductsController_GetProduct_ReturnOk()
        {
            //Arrange
            int productId = 1; 
            var product =A.Fake<Product>();
            var productDto = A.Fake<ProductDto>();
            A.CallTo(() =>_productService.GetProductAsync(productId)).Returns(product);
            A.CallTo(()=>_mapper.Map<ProductDto>(product)).Returns(productDto);
            //Act
            var result =await _productsController.GetProduct(productId);
            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task ProductsController_GetProduct_ReturnNotFound()
        {
            //Arrange
            Product product = null;
            A.CallTo(() => _productService.GetProductAsync(1)).Returns(product);
            //Act
            var result = await _productsController.GetProduct(1);
            //Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public async Task ProductsController_AddNewProduct_ReturnOk()
        {
            // Arrange
            var productDto = A.Fake<ProductDto>();
            var product = A.Fake<Product>();
            A.CallTo(() => _mapper.Map<ProductDto, Product>(productDto)).Returns(product);
            A.CallTo(() => _productService.AddProductAsync(product)).Returns(Task.FromResult(1));
            // Act
            var result = await _productsController.AddNewProduct(productDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(product);
        }
        [Fact]
        public async Task ProductsController_AddNewProduct_ReturnBadRequest()
        {
            // Arrange
            var productDto = A.Fake<ProductDto>();
            var product = A.Fake<Product>();
            A.CallTo(() => _mapper.Map<ProductDto, Product>(productDto)).Returns(product);
            A.CallTo(() => _productService.AddProductAsync(product)).Returns(Task.FromResult(0));
            // Act
            var result = await _productsController.AddNewProduct(productDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(400));
            
        }
        [Fact]
        public async Task ProductsController_UpdateProduct_ReturnOk()
        {
            //Arrange
            var productDto = A.Fake<ProductDto>();
            var product = A.Fake<Product>();
            A.CallTo(() => _mapper.Map<ProductDto, Product>(productDto)).Returns(product);
            A.CallTo(() => _productService.UpdateProductAsync(product)).Returns(Task.FromResult(product));
            //Act
            var result = await _productsController.UpdateProduct(productDto);
            //Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(product);
        }
        [Fact]
        public async Task ProductsController_UpdateProduct_ReturnNoFound()
        {
            //Arrange
            ProductDto productDto = null;
            Product product = null;
            A.CallTo(() => _mapper.Map<ProductDto, Product>(productDto)).Returns(product);
            A.CallTo(() => _productService.UpdateProductAsync(product)).Returns(Task.FromResult(product));
            //Act
            var result = await _productsController.UpdateProduct(productDto);
            //Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(404));
        }
    }
}
