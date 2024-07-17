using OrderSys.Core.Entities;
using OrderSys.Core.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Talabat.Core
{
    public interface IUnitOfWork :IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity :BaseEntity;
        Task<int> CompleteAsync();
    }
}
