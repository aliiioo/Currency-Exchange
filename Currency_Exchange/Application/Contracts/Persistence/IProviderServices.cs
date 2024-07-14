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
        public Task<Transaction> GetTransaction(int idTransaction);

        public Task<List<Transaction>> GetListTransactions(int fromIdAccount);
        public Task<List<Transaction>> GetListTransactionsForAdmin();

        public Task<bool> TransformCurrency(CreateTransactionDtos  transaction);




    }
}
