using OrderSys.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Entities
{
    public class User :BaseEntity
    {
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }
    }
}
