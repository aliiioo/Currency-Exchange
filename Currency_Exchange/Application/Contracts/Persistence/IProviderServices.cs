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

        public Task<List<TransactionDto>> GetListTransactions(int fromIdAccount);
        public Task<List<Transaction>> GetListTransactionsForAdmin();

        public Task<bool> TransformCurrency(CreateTransactionDtos  transactionVM, string username);
        public Task<bool> TransformToSelfAccountCurrency(CreateTransactionDtos  transactionVM, string username);
        public Task<bool> ConfirmTransaction(int transactionId, string username, bool isConfirm);




    }
}
