using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Entities
{
    public class Product :BaseEntity
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
