using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public decimal ExchangeRate { get; set; } = 0;
        public decimal FeePercentage { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

}
