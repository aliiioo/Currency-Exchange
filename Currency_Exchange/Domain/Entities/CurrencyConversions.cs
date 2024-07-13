using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CurrencyConversion
    {
        [Key]
        public int ConversionId { get; set; }
        [Required]
        public string FromCurrency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Required]
        public string ToCurrency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Required]
        public decimal Rate { get; set; } // Exchange rate from FromCurrency to ToCurrency
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

}
