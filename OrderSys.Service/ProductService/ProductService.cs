using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using Talabat.Core;

namespace OrderSys.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddProductAsync(Product product)
        {
            _unitOfWork.Repository<Product>().Add(product);

            var result = await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();

            return products;
        }
        public async Task<Product?> GetProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetAsync(id);

            return product;
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            var productFromDb = await _unitOfWork.Repository<Product>().GetAsync(product.Id);

            if (productFromDb == null) return null;

            productFromDb.Stock = product.Stock;
            productFromDb.Price  = product.Price;
            productFromDb.Name = product.Name;

            _unitOfWork.Repository<Product>().Update(productFromDb);

            await _unitOfWork.CompleteAsync();

            return productFromDb;
        }
    }
}

