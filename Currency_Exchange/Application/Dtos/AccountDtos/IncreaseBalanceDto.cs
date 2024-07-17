using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AccountDtos
{
    public class IncreaseBalanceDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }

    }
}
