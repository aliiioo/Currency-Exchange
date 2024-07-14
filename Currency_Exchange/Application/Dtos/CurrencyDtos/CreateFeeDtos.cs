using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.CurrencyDtos
{
    public class CreateFeeDtos
    {
        public decimal EndRange { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal PriceFee { get; set; }
    }
}
