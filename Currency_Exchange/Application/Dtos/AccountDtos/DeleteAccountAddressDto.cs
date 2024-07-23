namespace Application.Dtos.AccountDtos
{
    public class DeleteAccountAddressDto
    {
        public int AccountId { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }

        public DateTime CompleteTime { get; set; }

    }
}
