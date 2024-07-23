using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CurrencyDtos
{
    public class UpdateRateDtos
    {
        public int ExchangeRateId { get; set; }
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
    }
}
