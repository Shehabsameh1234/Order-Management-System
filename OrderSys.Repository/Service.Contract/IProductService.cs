using OrderSys.Core.Entities;

namespace OrderSys.Core.Service.Contract
{

    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<int> AddProductAsync(Product product);
        Task<int?> UpdateProductAsync(Product produc);

    }
}
