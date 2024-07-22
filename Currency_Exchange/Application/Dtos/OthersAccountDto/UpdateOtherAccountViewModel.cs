using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.OthersAccountDto
{
    public class UpdateOtherAccountViewModel
    {
        public int AccountId { get; set; }
        public string UserId { get; set; }
        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The field must be a number.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "The field must be 16 digits long.")]
        public string CartNumber { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;


    }
}
