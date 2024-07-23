using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<OthersAccount> OthersAccounts { get; set; } = new List<OthersAccount>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<DeletedAccount> DeletedAccounts { get; set; } = new List<DeletedAccount>();
    }

}
