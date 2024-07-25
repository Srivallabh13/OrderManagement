using System.ComponentModel.DataAnnotations;

namespace OrderManagement.DomainLayer.DTO
{
    public class RegisterDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        public bool IsClient { get; set; } = false;

    }
}
