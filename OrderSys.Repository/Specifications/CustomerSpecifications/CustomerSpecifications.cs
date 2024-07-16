using OrderSys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Specifications.CustomerSpecifications
{
    public class CustomerSpecifications :BaseSpecifications<Customer>
    {
        public CustomerSpecifications(int id) : base(c=>c.Id==id) 
        {
            Includes.Add(c => c.Orders);
            
        }
    }
}
