using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.OthersAccountDto
{
    public class OtherAccountViewModel
    {
        public int AccountId { get; set; }
        public string Currency { get; set; } = string.Empty; // 'USD', 'EUR', 'JPY', etc.
        public string AccountName { get; set; } = string.Empty;
        public string CartNumber { get; set; } = string.Empty;
    }
}
