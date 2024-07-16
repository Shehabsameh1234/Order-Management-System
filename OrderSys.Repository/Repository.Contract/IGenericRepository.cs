using OrderSys.Core.Entities;
using OrderSys.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Repository.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int id);
        Task<T?> GetWithSpecAsync(Ispecifications<T> spec);
        Task<bool> CheckUserNameExsist(Ispecifications<T> spec);
        Task<T?> GetUserByUserName(Ispecifications<T> spec);
        Task<IReadOnlyList<T>> GetAllAsync();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
