using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Entities.Enums
{
    public enum UserRole
    {
        [EnumMember(Value = "admin")]
        Admin,

        [EnumMember(Value = "customer")]
        Customer,
    }
}
