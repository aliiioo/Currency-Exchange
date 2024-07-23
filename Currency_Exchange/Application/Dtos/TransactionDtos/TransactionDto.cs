using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TransactionDtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string FromAccountId { get; set; }
        public string ToOtherAccountId { get; set; }
        public string ToAccountId { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; } 
        public decimal ConvertedAmount { get; set; } 
        public decimal ExchangeRate { get; set; }
        public decimal Fee { get; set; }
        [Required]
        public string Status { get; set; } // 'Pending', 'Completed', 'Cancelled'
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; }
    }
}
