using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CurrencyDtos
{
    public class CreateFeeDtos
    {
        public int CurrencyId { get; set; }
        public decimal EndRange { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        [Range(0,40)]
        public decimal PriceFee { get; set; }
    }
}
