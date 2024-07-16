using OrderSys.Core.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Order_Management_System.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d)(?=.*[a-zA-Z]).{6,}$",
            ErrorMessage = "password must contain at least one number , one upper case charachter ,one alphanumeric")]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } =null!;
    }
}
