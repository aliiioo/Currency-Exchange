using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Dtos.TransactionDtos;

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

        public Task<string> GetNameAccountForTransaction(int accountId);
        public Task<string> GetOtherNameAccountForTransaction(int accountId);







    }
}
