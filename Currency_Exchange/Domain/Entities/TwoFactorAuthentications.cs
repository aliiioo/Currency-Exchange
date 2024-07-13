using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TwoFactorAuthentication
    {
        [Key]
        public int TwoFactorId { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ApplicationUser User { get; set; }
    }

}
