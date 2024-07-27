using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.OthersAccountDtos
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [MinLength(10)]
        [MaxLength(14)]
        public string Phone { get; set; }

        [Required]
        [MinLength(8)]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
