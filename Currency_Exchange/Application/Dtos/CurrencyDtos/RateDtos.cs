using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CurrencyDtos
{
    public class RateDtos
    {
        public int CurrencyId { get; set; }
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }
        [Required]
        public decimal Rate { get; set; } = 0;
    }
}
