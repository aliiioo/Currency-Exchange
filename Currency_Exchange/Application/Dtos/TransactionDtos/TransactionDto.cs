using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.TransactionDtos
{
    public class TransactionDto
    {
        public int TransactionId { get;}
        public string FromAccount { get; }
        public string ToAccount { get; }
        public string FromCurrency { get; }
        public string ToCurrency { get; }
        public decimal Amount { get; } 
        public decimal ConvertedAmount { get; } 
        public decimal ExchangeRate { get;}
        public decimal Fee { get; }
        [Required]
        public string Status { get; } // 'Pending', 'Completed', 'Cancelled'
        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime? CompletedAt { get; }
    }
}
