using Application.Dtos.TransactionDtos;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IProviderServices
    {
        public Task<TransactionDto> GetTransactionAsync(int idTransaction);
        public Task<ConfirmTransactionDto> GetConfirmTransactionAsync(int idTransaction,string userId);
        public Task<TransactionDetailDto> GetDetailTransactionAsync(int idTransaction,string userId);
        public Task<List<TransactionDto>> GetListTransactionsAsync(int fromIdAccount);
        public Task<List<UsersTransactionsDto>> GetListTransactionsForAdminAsync();

        public Task<bool> CancelTransactionAsync(int transactionId);
        public Task<int> TransformCurrencyAsync(CreateTransactionDtos  transactionVM, string username);
        public Task<bool> ConfirmTransactionAsync(int transactionId, string username, bool isConfirm);
        public Task<List<Transaction>> CanceledPendingTransactionsByTimePassAsync(int min);
        public Task<bool> CheckDailyTransactionThreshold(string userId,int sourceAccountId,decimal transactionAmount,string transactionCurrency);
        public Task<string> GetNameAccountForTransaction(int accountId);
        public Task<string> GetOtherNameAccountForTransaction(int accountId);







    }
}
