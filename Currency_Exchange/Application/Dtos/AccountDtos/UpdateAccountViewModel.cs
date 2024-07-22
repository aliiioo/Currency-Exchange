using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AccountDtos
{
    public class UpdateAccountViewModel
    {
        public int AccountId { get; set; }
        public string UserId { get; set; }
        
        public string AccountName { get; set; } = string.Empty;
        public string Currency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        public decimal Balance { get; set; }
    }
}
