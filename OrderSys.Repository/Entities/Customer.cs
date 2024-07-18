﻿

namespace OrderSys.Core.Entities
{
    public class Customer :BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
