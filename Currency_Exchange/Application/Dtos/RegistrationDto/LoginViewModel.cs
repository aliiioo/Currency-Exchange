using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.OthersAccountDtos
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
