using OrderSys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Service.Contract
{
    public interface IAuthService
    {
       Task<string> CreateTokenAsync(User user);  
    }
}
