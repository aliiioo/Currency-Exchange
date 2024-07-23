using Application.Dtos.TransactionDtos;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IProviderServices
    {
        public Task<TransactionDto> GetTransaction(int idTransaction);
        public Task<ConfirmTransactionDto> GetConfirmTransaction(int idTransaction,string userId);
        public Task<List<TransactionDto>> GetListTransactions(int fromIdAccount);
        public Task<List<TransactionDto>> GetListTransactionsForAdmin();

        public Task<int> TransformCurrency(CreateTransactionDtos  transactionVM, string username);
        public Task<int> TransformToSelfAccountCurrency(CreateTransactionDtos  transactionVM, string username);
        public Task<bool> ConfirmTransaction(int transactionId, string username, bool isConfirm);
        public Task<List<Transaction>> CanceledPendingTransactionsByTimePass(int min);
        public Task<bool> CheckMaxOfTransaction(string userId,decimal price);
        public Task<string> GetNameAccountForTransaction(int accountId);
        public Task<string> GetOtherNameAccountForTransaction(int accountId);







    }
}
