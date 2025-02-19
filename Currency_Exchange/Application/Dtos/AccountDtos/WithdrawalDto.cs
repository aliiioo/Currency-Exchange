﻿using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AccountDtos
{
    public class WithdrawalDto
    {
        public int AccountId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "The value must be 0$ or higher.")]
        public decimal  Amount { get; set; }
    }
}
