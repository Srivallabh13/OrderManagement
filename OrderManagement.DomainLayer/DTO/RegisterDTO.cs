using System.ComponentModel.DataAnnotations;

namespace OrderManagement.DomainLayer.DTO
{
    public class RegisterDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        public bool IsClient { get; set; } = false;

    }
}
