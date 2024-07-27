using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DeletedAccount
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AccountId { get; set; }
        [MaxLength(1200)] public string? Address { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CompleteTime { get; set; }
        public bool Accepted { get; set; }=false;


        [ForeignKey("UserId")]
        public ApplicationUser User{ get; set; }


    }
}
