using Microsoft.EntityFrameworkCore;
using OrderSys.Core.Entities;
using OrderSys.Core.Repository.Contract;
using OrderSys.Core.Specifications;
using OrderSys.Repository.Data;


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

        public async Task<bool> CheckUserNameExsist(Ispecifications<T> spec)
       => await  ApplySpecifications(spec).AnyAsync();

        public async Task<T?> GetUserByUserName(Ispecifications<T> spec)
        =>await ApplySpecifications(spec).FirstOrDefaultAsync();
        public async Task<T?> GetWithSpecAsync(Ispecifications<T> spec)
        => await ApplySpecifications(spec).FirstOrDefaultAsync();
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecifications<T> spec)
        =>await  ApplySpecifications(spec).ToListAsync();
        public void Add(T entity)
        => _dbContext.Set<T>().Add(entity); 

        public void Delete(T entity)
        => _dbContext.Remove(entity);

        public void Update(T entity)
        => _dbContext.Update(entity);


        private IQueryable<T> ApplySpecifications(Ispecifications<T> spec)
        {
            return SpecificationEvaluater<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

      
    }
}
