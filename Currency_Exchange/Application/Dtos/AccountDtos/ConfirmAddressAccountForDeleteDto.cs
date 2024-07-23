using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AccountDtos
{
    public class ConfirmAddressAccountForDeleteDto
    {
        public int TransactionId { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public bool IsConfirm { get; set; }
    }
}
