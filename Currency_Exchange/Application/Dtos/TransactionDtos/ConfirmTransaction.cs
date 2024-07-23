using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.TransactionDtos
{
    public class ConfirmTransactionDto
    {
        public int TransactionId { get; set; }
        public string FromAccountId { get; set; }
        public string? ToOtherAccountId { get; set; }=string.Empty;
        public string? ToAccountId { get; set; } = string.Empty;
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Fee { get; set; }
        [Required]
        public string Status { get; set; } // 'Pending', 'Completed', 'Cancelled'
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsConfirm { get; set; }
    }
}
