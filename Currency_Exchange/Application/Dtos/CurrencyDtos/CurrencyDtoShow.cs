﻿using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CurrencyDtos
{
    public class CurrencyDtoShow
    {
        public int CurrencyId { get; set; }
        [MaxLength(4)]
        [MinLength(2)]
        public string CurrencyCode { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [MaxLength(100)]
        [MinLength(1)]
        [Required]
        public string CurrencyName { get; set; }
    }
}
