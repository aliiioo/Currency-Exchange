using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ExchangeRate
    {
        [Key]
        public int ExchangeRateId { get; set; }
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

}
