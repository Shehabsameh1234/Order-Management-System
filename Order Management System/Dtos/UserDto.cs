using OrderSys.Core.Entities.Enums;

namespace Order_Management_System.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } =null!;
    }
}
