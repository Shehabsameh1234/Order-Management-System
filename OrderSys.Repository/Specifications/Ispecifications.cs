using OrderSys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Specifications
{
    public interface Ispecifications<T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Criteria { get; set; }
    }
}
