﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ExchangeRate
    {
        [Key]
        public int ExchangeRateId { get; set; }
        public int CurrencyId { get; set; }
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }
        [Required]
        [Range(0,40)]
        public decimal Rate { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.Now;




        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }





    }

}
