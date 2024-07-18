using OrderSys.Core.Entities;
using OrderSys.Core.Specifications;


namespace OrderSys.Core.Repository.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int id);
        Task<T?> GetWithSpecAsync(Ispecifications<T> spec);

        Task<bool> CheckUserNameExsist(Ispecifications<T> spec);
        Task<T?> GetUserByUserName(Ispecifications<T> spec);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecifications<T> spec);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
