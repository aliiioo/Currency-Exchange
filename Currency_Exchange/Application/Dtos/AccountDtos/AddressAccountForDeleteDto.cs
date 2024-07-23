using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AccountDtos
{
    public class AddressAccountForDeleteDto
    {
        public int AccountId { get; set; }
        [MaxLength(1200)]
        [Required]
        public string Address { get; set; }
        



    }
}
