using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Entities
{
    public class Customer :BaseEntity
    {
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
