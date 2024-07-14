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
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal ConvertedAmount { get; set; } = 0;
        public decimal ExchangeRate { get; set; } = 0;
        public decimal Fee { get; set; } = 0;
        [Required]
        public string Status { get; set; } // 'Pending', 'Completed', 'Cancelled'
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
    }
}
