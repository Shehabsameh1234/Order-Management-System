using OrderSys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Specifications.OrderSpecifications
{
    public class OrderSpecifications :BaseSpecifications<Order>
    {
        public OrderSpecifications(int id):base(o=>o.Id==id)
        {
            Includes.Add(o => o.OrderItems);
        }
        public OrderSpecifications() : base()
        {
            Includes.Add(o => o.OrderItems);
        }
    }
}
