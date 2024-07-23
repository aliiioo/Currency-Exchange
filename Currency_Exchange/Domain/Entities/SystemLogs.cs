using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SystemLog
    {
        [Key]
        public int LogId { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ApplicationUser User { get; set; }
    }

}
