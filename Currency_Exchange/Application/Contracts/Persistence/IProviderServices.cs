﻿using Application.Dtos.TransactionDtos;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IProviderServices
    {
        public Task<TransactionDto> GetTransaction(int idTransaction);
        public Task<ConfirmTransactionDto> GetConfirmTransaction(int idTransaction,string userId);
        public Task<TransactionDetailDto> GetDetailTransaction(int idTransaction,string userId);
        public Task<List<TransactionDto>> GetListTransactions(int fromIdAccount);
        public Task<List<UsersTransactionsDto>> GetListTransactionsForAdmin();

        public Task<bool> CancelTransaction(int transactionId);
        public Task<int> TransformCurrency(CreateTransactionDtos  transactionVM, string username);
        public Task<bool> ConfirmTransaction(int transactionId, string username, bool isConfirm);
        public Task<List<Transaction>> CanceledPendingTransactionsByTimePass(int min);
        public Task<bool> CheckMaxOfTransaction(string userId,int accountId,decimal price,string currency);
        public Task<string> GetNameAccountForTransaction(int accountId);
        public Task<string> GetOtherNameAccountForTransaction(int accountId);







    }
}
