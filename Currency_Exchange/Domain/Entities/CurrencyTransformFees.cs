using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CurrencyTransformFees
    {
        [Key]
        public int FeeId { get; set; }
        public int CurrencyId { get; set; }
        [Required]
        public string FromCurrency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Required]
        public string ToCurrency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Required]
        public decimal StartRange { get; set; } = 0;
        public decimal EndRange { get; set; } = 0;
        [Range(0,40)]
        public decimal PriceFee { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

      

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }




    }
}
