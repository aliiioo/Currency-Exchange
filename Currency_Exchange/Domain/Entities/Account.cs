using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string UserId { get; set; }
        public string Currency { get; set; } // 'USD', 'EUR', 'JPY', etc.
        public decimal Balance { get; set; } = 0;
        public string AccountName { get; set; } = string.Empty;
        public string CartNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

    public enum AccountTypeEnum
    {
        Self,
        Others
    }


}
