namespace Application.Dtos.AccountDtos
{
    public class ConfirmAddressAccountForDeleteDto
    {
        public int AccountId { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public bool IsConfirm { get; set; }
    }
 }  
