using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Application.Dtos.OthersAccountDto
{
    public class CreateOtherAccountViewModel
    {
        public string UserId { get; set; }
        [MaxLength(4)]
        [MinLength(1)]
        public string Currency { get; set; } // 'USD', 'EUR', 'JPY', etc.

        public string AccountName { get; set; } = string.Empty;

        public string CartNumber { get; set; } = string.Empty;



    }
}
