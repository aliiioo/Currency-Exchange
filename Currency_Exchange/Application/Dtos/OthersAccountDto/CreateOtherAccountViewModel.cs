using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.OthersAccountDto
{
    public class CreateOtherAccountViewModel
    {
        public string UserId { get; set; }
        [MaxLength(4)]
        [MinLength(1)]
        public string Currency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Required]
        public string AccountName { get; set; } = string.Empty;
        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The field must be a number.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "The field must be 16 digits long.")]
        public string CartNumber { get; set; } = string.Empty;



    }
}
