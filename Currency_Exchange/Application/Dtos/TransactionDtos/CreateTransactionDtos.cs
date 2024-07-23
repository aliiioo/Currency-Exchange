namespace Application.Dtos.TransactionDtos
{
    public class CreateTransactionDtos
    {
        public string Username { get; set; }
        public int SelfAccountId { get; set; }
        public int OthersAccountId { get; set; }
        public string OthersAccountIdAsString { get; set; }
        public string FromCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}
