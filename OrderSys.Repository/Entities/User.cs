using OrderSys.Core.Entities.Enums;


namespace OrderSys.Core.Entities
{
    public class User :BaseEntity
    {
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }
    }
}
