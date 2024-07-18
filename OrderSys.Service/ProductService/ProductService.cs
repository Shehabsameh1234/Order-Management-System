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

        public async Task<int?> UpdateProductAsync(Product product)
        {
            var user = await _unitOfWork.Repository<Product>().GetAsync(product.Id);

            if (user == null) return null;

            user.Stock = product.Stock;
            user.Price  = product.Price;
            user.Name = product.Name;

            _unitOfWork.Repository<Product>().Update(user);

            var result = await _unitOfWork.CompleteAsync();

            return result;
        }
    }
}

