using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class OthersAccount
    {
        [Key]
        public int AccountId { get; set; }
        public string UserId { get; set; }
        public string Currency { get; set; } = string.Empty; // 'USD', 'EUR', 'JPY', etc.

        public string AccountName { get; set; } = string.Empty;
        [StringLength(16)]
        public string CartNumber { get; set; } = string.Empty;
        [Range(0,double.MaxValue)]
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }=new List<Transaction>();
    }
}
