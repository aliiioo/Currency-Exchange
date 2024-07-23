using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        [MaxLength(20)]
        [MinLength(1)]
        [Required]
        public string CurrencyCode { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [MaxLength(100)]
        [MinLength(1)]
        [Required]
        public string CurrencyName { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public List<CurrencyExchangeFees> CurrencyExchangeFees { get; set; } = new List<CurrencyExchangeFees>();
        public List<CurrencyTransformFees> CurrencyTransformFees { get; set; } = new List<CurrencyTransformFees>();
        public List<ExchangeRate> ExchangeRate { get; set; }= new List<ExchangeRate>();
       



    }

}
