using OrderSys.Core.Entities;


namespace OrderSys.Core.Service.Contract
{
    public interface IAuthService
    {
       Task<string> CreateTokenAsync(User user);  
    }
}
