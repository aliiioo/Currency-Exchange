using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Transaction
    {
        public Transaction()
        {
            CreatedAt= DateTime.UtcNow;
        }

        [Key]
        public int TransactionId { get; set; }
        public string? UserId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        //public int? ToOtherAccountId { get; set; }
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        public decimal Amount { get; set; } = 0;

        public decimal UserBalance { get; set; } = 0;
        public decimal DeductedAmount { get; set; } = 0;
        [Range(0,40)]
        public decimal? ExchangeRate { get; set; } = 0;
        [Range(0, 40)]
        public decimal Fee { get; set; } = 0;
        [Required]
        public StatusEnum Status { get; set; } // 'Pending', 'Completed', 'Cancelled'
        public DateTime CreatedAt { get; init; }
        public DateTime? CompletedAt { get; set; }
        public bool Outer { get; set; }=true;

        // Navigation properties
        [ForeignKey("FromAccountId")]
        public Account FromAccount { get; set; }
        [ForeignKey("ToAccountId")]
        public Account? ToAccount { get; set; }

        // [ForeignKey("ToOtherAccountId")]
        // public OthersAccount? ToOthersAccount{ get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }


    }


    public enum StatusEnum
    {
        Pending,
        Completed,
        Cancelled
    }


}
