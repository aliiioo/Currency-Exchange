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
        public string AccountName { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;


    }
}
