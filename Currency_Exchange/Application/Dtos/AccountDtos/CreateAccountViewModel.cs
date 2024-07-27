using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AccountDtos
{
    public class CreateAccountViewModel
    {
        public string UserId { get; set; }
        [MaxLength(4)]
        [MinLength(1)]
        public string Currency { get; set; } // 'USD', 'EUR', 'JPY', etc.

        public string AccountName { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "The value must be 50$ or higher.")]
        public decimal Balance { get; set; }
    }
}
