using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.TransactionDtos
{
    public class ConfirmTransactionDto
    {
        public int TransactionId { get; set; }
        public bool IsConfirm { get; set; }
    }
}
