using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string UserId { get; set; }
        public string Currency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        [Range(0,double.MaxValue)]
        public decimal Balance { get; set; } = 0;
        [MaxLength(300)] 
        public decimal OutBalance { get; set; }

        public string AccountName { get; set; } = string.Empty;
        [StringLength(16)]
        public string CartNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }=false;
        public bool IsActive { get; set; }=true;

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public enum AccountTypeEnum
    {
        Self,
        Others
    }


}
