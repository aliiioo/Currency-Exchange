using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AccountDtos
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public string Currency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        public decimal Balance { get; set; } = 0;
        public string AccountName { get; set; } = string.Empty;
        public string CartNumber { get; set; } = string.Empty;
    }
}
