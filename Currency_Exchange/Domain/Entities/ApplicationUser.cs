using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        public string FullName { get; set; }
        public bool TwoFactorEnabled { get; set; } = false;
        public decimal DailyWithdrawalLimit { get; set; } = 10000.00m;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<OthersAccount> OthersAccounts { get; set; } = new List<OthersAccount>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<TwoFactorAuthentication> TwoFactorAuthentications { get; set; } = new List<TwoFactorAuthentication>();
        public ICollection<DeletedAccount> DeletedAccounts { get; set; } = new List<DeletedAccount>();
    }

}
