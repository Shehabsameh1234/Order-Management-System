using OrderSys.Core.Entities;
using OrderSys.Core.Repository.Contract;
using OrderSys.Repository;
using OrderSys.Repository.Data;
using System.Collections;
using Talabat.Core;


namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
       
        private readonly Hashtable _repository;
        private readonly OrderManagementDbContext _dbContext;

        public UnitOfWork(OrderManagementDbContext dbContext)
        {
             
            _repository = new Hashtable();
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //get the name of t entity like (product)
            var key =typeof(TEntity).Name;
            if(!_repository.ContainsKey(key))
            {
                var repository=new GenericRepository<TEntity>(_dbContext);
                _repository.Add(key, repository);
            }
            return _repository[key] as IGenericRepository<TEntity>;
        }
        public async Task<int> CompleteAsync()
         => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        =>await _dbContext.DisposeAsync();   
    }
}
