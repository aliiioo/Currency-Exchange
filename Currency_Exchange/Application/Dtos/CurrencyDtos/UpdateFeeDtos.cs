using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CurrencyDtos
{
    public class UpdateFeeDtos
    {
        public int FeeId { get; set; }
        public string FromCurrency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Required]
        public string ToCurrency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Required]
        public decimal StartRange { get; set; } = 0;
        public decimal EndRange { get; set; } = 0;
        public decimal PriceFee { get; set; }



    }
}
