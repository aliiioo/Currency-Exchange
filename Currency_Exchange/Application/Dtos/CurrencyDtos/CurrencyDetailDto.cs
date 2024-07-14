using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.CurrencyDtos
{
    public class CurrencyDetailDto
    {
        [MaxLength(4)]
        [MinLength(2)]
        public string CurrencyCode { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [MaxLength(100)]
        [MinLength(1)]
        [Required]
        public string CurrencyName { get; set; }

        public List<CurrencyExchangeFees> CurrencyExchangeFees { get; set; } = new List<CurrencyExchangeFees>();
        public List<CurrencyTransformFees> CurrencyTransformFees { get; set; }=new List<CurrencyTransformFees>();
        public ExchangeRate ExchangeRate { get; set; }=new ExchangeRate();





    }
}
