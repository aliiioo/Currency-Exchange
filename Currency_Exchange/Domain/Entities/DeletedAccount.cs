using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DeletedAccount
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public DateTime CompleteTime { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User{ get; set; }


    }
}
