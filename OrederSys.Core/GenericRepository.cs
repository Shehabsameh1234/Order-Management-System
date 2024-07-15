using Microsoft.EntityFrameworkCore;
using OrderSys.Core.Entities;
using OrderSys.Core.Repository.Contract;
using OrderSys.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly OrderManagementDbContext _dbContext;

        public GenericRepository(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        =>await _dbContext.Set<T>().ToListAsync();

        public async Task<T?> GetAsync(int id)
        => await _dbContext.Set<T>().FindAsync(id);

        public void Add(T entity)
        => _dbContext.Set<T>().Add(entity); 

        public void Delete(T entity)
        => _dbContext.Remove(entity);

        public void Update(T entity)
        => _dbContext.Update(entity);
    }
}
