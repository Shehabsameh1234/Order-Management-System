using OrderSys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Specifications.UserSpecifications
{
    public class UserSpecifications : BaseSpecifications<User>
    {
        public UserSpecifications(string userName):base(u=>u.Username==userName)
        {
            
        }
    }
}
