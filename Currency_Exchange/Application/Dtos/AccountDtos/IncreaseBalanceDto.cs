using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AccountDtos
{
    public class IncreaseBalanceDto
    {
        public int AccountId { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "The value must be 0$ or higher.")]
        public decimal Amount { get; set; }

    }
}
