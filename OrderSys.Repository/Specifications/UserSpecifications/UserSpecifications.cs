using OrderSys.Core.Entities;

namespace OrderSys.Core.Specifications.UserSpecifications
{
    public class UserSpecifications : BaseSpecifications<User>
    {
        public UserSpecifications(string userName):base(u=>u.Username==userName)
        {
            
        }
    }
}
