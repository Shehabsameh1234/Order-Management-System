using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;


namespace Order_Management_System.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper; 
        }
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
        {
            var products =await _productService.GetAllProductsAsync();

            if (products == null) return NotFound(new ApisResponse(404));

            var mappedProducts= _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            return Ok(mappedProducts);   
        }
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product == null) return NotFound(new ApisResponse(404));

            var mappedProduct = _mapper.Map<Product, ProductDto>(product);

            return Ok(mappedProduct);
        }

        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddNewProduct(ProductDto productDto)
        {
            var mappedModel = _mapper.Map<ProductDto,Product>(productDto);

           var result = await _productService.AddProductAsync(mappedModel);

           if (result == 0) return BadRequest(new ApisResponse(400));

           return Ok(mappedModel); 
        }
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductDto productDto)
        {
            var mappedModel = _mapper.Map<ProductDto, Product>(productDto);

            var result =await _productService.UpdateProductAsync(mappedModel);

            if (result ==null) return BadRequest(new ApisResponse(400));

            return Ok(mappedModel);
        }



    }
}
