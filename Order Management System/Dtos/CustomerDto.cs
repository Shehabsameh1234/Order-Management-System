using System.ComponentModel.DataAnnotations;

namespace Order_Management_System.Dtos
{
    public class CustomerDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
