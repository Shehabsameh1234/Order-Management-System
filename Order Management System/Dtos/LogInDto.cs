using System.ComponentModel.DataAnnotations;

namespace Order_Management_System.Dtos
{
    public class LogInDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
