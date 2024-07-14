using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<CurrencyExchangeFees> CurrencyExchangeFees { get; set; }
        public List<CurrencyTransformFees> CurrencyTransformFees { get; set; }
        public List<ExchangeRate> ExchangeRate { get; set; }
       



    }

}
